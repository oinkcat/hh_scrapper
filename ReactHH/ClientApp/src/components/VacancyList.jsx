import React from 'react';
import { Vacancy } from './Vacancy';

export class VacancyList extends React.Component {

    constructor(props) {
        const DATA_URL = '/api/vacancies/list';

        super(props);

        this.state = {
            loading: true,
            vacancies: []
        };

        fetch(DATA_URL)
            .then(resp => resp.json())
            .then(loadedData => {
                let { vacancies, isFallback } = loadedData;
                this.setState({ loading: false, vacancies: vacancies });
                this.props.onLoaded(vacancies, isFallback);
            })
            .catch(reason => this.props.onError(reason));
    }

    renderLoader() {
        return <h4>Loading...</h4>;
    }

    renderList() {
        const filterMode = this.props.mode;

        const vacanciesToShow = this.state.vacancies.filter(info => {
            const notApplicable = filterMode === 0;
            const onlyActive = filterMode === 1;

            return notApplicable || info.isActive === onlyActive;
        }).sort((a, b) => a.isActive < b.isActive ? 1 : -1);

        return (
            <div className="vacancy-list">
                {vacanciesToShow.map(vacancy => {
                    return <Vacancy key={vacancy.id} info={vacancy} />;
                })}
            </div>
        );
    }

    render() {
        return this.state.loading ? this.renderLoader() : this.renderList();
    }
}