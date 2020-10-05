-- Vacancies
CREATE TABLE vacancies (
	id INTEGER PRIMARY KEY AUTOINCREMENT,
	url TEXT(128),
	found_date text (16),
	update_date text(16),
	title text(128),
	employer text(128),
	place text(64),
	salary text(32)
);

CREATE INDEX ix_found_date ON vacancies (found_date ASC);
CREATE INDEX ix_place ON vacancies (place ASC);
CREATE INDEX ix_salary ON vacancies (salary ASC);
CREATE INDEX ix_update_date ON vacancies (update_date ASC);

-- Vacancy tags (requirements)
CREATE TABLE tags (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name text(32)
);

-- Vacancies to tags
CREATE TABLE vacancies_tags (
	vacancy_id INTEGER REFERENCES vacancies (id) ON DELETE CASCADE,
	tag_id INTEGER REFERENCES tags (id) ON DELETE CASCADE
);

-- Statistics
CREATE TABLE stats (
	date text(16) PRIMARY KEY,
	total_count integer,
	new_count integer
);