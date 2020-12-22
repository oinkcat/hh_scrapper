#!/usr/bin/python3
# -*- coding: utf-8 -*-
import urllib.request
import urllib.parse
import time
import datetime
import sqlite3
import json
import bs4
import hh_db_utils

HOST_NAME = 'https://hh.ru'
CONFIG_PATH = './config.json'
DB_PATH = './hh.db'

class Vacancy:
	""" Vacancy basic info """

	def __init__(self, id, title):
		self.id = str(id)
		self.title = title
		self.employer = None
		self.place = None
		self.salary = None
		self.tags = list()
		
	def __str__(self):
		return "\n".join([
			self.id,
			'{0} - {1}'.format(self.title, self.employer),
			self.place,
			self.salary,
			', '.join(self.tags)
		])


def scrap_vacancies():
	""" Load, parse and save vacancy information from hh.ru """

	hh_db_utils.ensure_db_created(DB_PATH)

	# Load query results
	page_num = 1
	url = format_query_url()
	parsed_items = []

	while url is not None:
		print('Loading page %d...' % page_num)

		with urllib.request.urlopen(url) as list_flo:
			soup = bs4.BeautifulSoup(list_flo, 'html.parser')

		# Parse items on page
		print('Parsing...')

		vacancy_nodes = soup.find_all('div', class_='vacancy-serp-item')
		page_items = list(map(get_vacancy_info_from_node, vacancy_nodes))
		parsed_items.extend(page_items)

		next_page_link = soup.find('a', { 'data-qa': 'pager-next' })

		url = '%s/%s' % (HOST_NAME, next_page_link['href']) \
			if next_page_link is not None \
			else None

		page_num += 1
	
	# Save to database
	print('Saving...')
	save_to_db(parsed_items)


def format_query_url():
	""" Construct query URL """

	with open(CONFIG_PATH, encoding='utf-8') as f_config:
		config = json.load(f_config)
		
		qs_params = {
			'text': config['Query'],
			'area': 1,
			'salary': '',
			'currency_code': 'RUR',
			'experience': 'doesNotMatter',
			'schedule': 'remote',
			'order_by': 'relevance',
			'search_period': '',
			'items_on_page': 100,
			'no_magic': 'true'
		}
		qs = urllib.parse.urlencode(qs_params)

	return '%s/search/vacancy?st=searchVacancy&%s' % (HOST_NAME, qs)


def get_vacancy_info_from_node(node):
	""" Extract vacancy information from html node """

	# Basic info
	title_elem = node.find('a', attrs={"data-qa": "vacancy-serp__vacancy-title"})
	link = urllib.parse.splitquery(title_elem['href'])[0]
	employer_elem = node.find('a', attrs={"data-qa": "vacancy-serp__vacancy-employer"})
	place_elem = node.find('span', attrs={"data-qa": "vacancy-serp__vacancy-address"})
	salary_elem = node.find('span', attrs={"data-qa": "vacancy-serp__vacancy-compensation"})

	# Details
	details_flo = urllib.request.urlopen(title_elem['href'])
	details_soup = bs4.BeautifulSoup(details_flo, 'html.parser')
	tags = details_soup.find_all('span', class_="bloko-tag__section_text")

	vacancy = Vacancy(link, title_elem.text)
	vacancy.employer = employer_elem.text if employer_elem is not None else '?'
	vacancy.place = place_elem.text
	vacancy.salary = salary_elem.text if salary_elem is not None else '?'
	vacancy.tags = list(map(lambda t: t.text, tags))

	return vacancy
		

def save_to_db(vacancies):
	""" Save parsed info to database """

	db = sqlite3.Connection(DB_PATH)
	
	today = datetime.date.today().isoformat()
	n_new, n_updated = import_vacancies(db, today, vacancies)	
	
	n_total = n_new + n_updated
	save_stats(db, today, n_total, n_new)
	
	print('Total: {0}, new: {1}, updated: {2}'.format(n_total, n_new, n_updated))
	

def import_vacancies(db, date, part):
	""" Import list of found vacancies """	

	num_new, num_updated = 0, 0
	
	for info in part:
		cur = db.execute('SELECT id FROM vacancies WHERE url = ?', (info.id,))
		id_row = cur.fetchone()
		
		if id_row is None:
			# Item is new - insert into DB
			insert_data = [
				info.id,
				date,
				date,
				info.title,
				info.employer,
				info.place,
				info.salary
			]
			cur = db.execute('INSERT INTO vacancies (' +
								'url, found_date, update_date, title, ' +
								'employer, place, salary'
								') VALUES (?, ?, ?, ?, ?, ?, ?)', insert_data)
			import_tags(db, info.tags, cur.lastrowid)
			
			num_new += 1
		else:
			# Existing item - just update date
			db.execute('UPDATE vacancies SET update_date = ? WHERE id = ?', \
					   (date, id_row[0],))
			num_updated += 1
			
	db.commit()
	
	return num_new, num_updated
		

def import_tags(db, tags, vacancy_id):
	""" Import tags to its' table """
	
	for tag in tags:
		# Find tag with name
		cur = db.execute('SELECT id FROM tags WHERE name = ?', (tag,))
		id_row = cur.fetchone()
		
		if id_row is None:
			# Tag not exists - insert
			cur = db.execute('INSERT INTO tags (name) VALUES (?)', (tag,))
			id_row = (cur.lastrowid,)
			
		# Link tag with vacancy
		db.execute('INSERT INTO vacancies_tags (vacancy_id, tag_id) ' +
				   'VALUES (?, ?)', (vacancy_id, id_row[0]))
	

def save_stats(db, date, n_total, n_new):
	""" Save statistics """

	cur = db.execute('SELECT * FROM stats WHERE date = ?', (date,))
	old_stats_row = cur.fetchone()
	
	today_new = n_new
	if old_stats_row is not None:
		today_new += old_stats_row[2]
	
	db.execute('REPLACE INTO stats VALUES (?, ?, ?)', (date, n_total, today_new))
	db.commit()

if __name__ == '__main__':
	scrap_vacancies()