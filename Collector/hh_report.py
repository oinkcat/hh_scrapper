import sqlite3

DB_PATH = '/data/hh.db'
OUT_FILE_VACANCIES = '/var/www/data_sets/vacancies.txt'
OUT_FILE_STATS = '/var/www/data_sets/vacancies_stats.txt'

QUERY_VACANCIES = '''SELECT v.*, t.name
  FROM vacancies AS v
    INNER JOIN vacancies_tags AS vt ON v.id = vt.vacancy_id
    INNER JOIN tags AS t ON vt.tag_id = t.id'''

QUERY_STATS = 'SELECT * FROM stats'

def make_report():
    db = sqlite3.Connection(DB_PATH)
    
    # Vacancies
    cur_v = db.execute(QUERY_VACANCIES)
    tsv_data_v = ["\t".join(map(str, row)) + "\n" for row in cur_v.fetchall()]

    with open(OUT_FILE_VACANCIES, 'w') as out_file:
        out_file.writelines(tsv_data_v)
        
    # Stats
    cur_s = db.execute(QUERY_STATS)
    tsv_data_s = ["\t".join(map(str, row)) + "\n" for row in cur_s.fetchall()]
    
    with open(OUT_FILE_STATS, 'w') as out_file:
    	out_file.writelines(tsv_data_s)
    
try:
	make_report()
	print('Report done')
except:
    print('ERROR OCCURED!')
