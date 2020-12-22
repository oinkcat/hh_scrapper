# -*- coding: utf-8 -*-
import sqlite3

DEFAULT_DB_PATH = './hh.db'
DB_DDL_FILE = './hh_db.sql'

def ensure_db_created(db_path):
    """ Check DB existence and create if it doesn't exist """

    db = sqlite3.Connection(db_path)

    cur = db.execute("SELECT 1 FROM sqlite_master"
                     "  WHERE type = 'table' AND name = 'vacancies'")
    if cur.fetchone() is not None:
        print(f'Database {DEFAULT_DB_PATH} already exists')
    else:
        __create_db_tables(db)
        print(f'Database {DEFAULT_DB_PATH} created')

    db.close()


def __create_db_tables(db):
    """ Create database structure """

    with open(DB_DDL_FILE) as f_ddl:
        while True:
            ddl_statement = __read_create_statement(f_ddl)

            if ddl_statement is None:
                break

            db.execute(ddl_statement)

    db.commit()


def __read_create_statement(f):
    """ Read create object statement from DDL file """

    sql_buffer = []

    while True:
        line = f.readline()
        if len(line) == 0:
            break

        t_line = line.strip()
        if len(t_line) == 0 or line.startswith('--'):
            continue

        sql_buffer.append(t_line)

        if t_line.endswith(';'):
            break

    return ' '.join(sql_buffer) if len(sql_buffer) > 0 else None

if __name__ == '__main__':
    ensure_db_created(DEFAULT_DB_PATH)