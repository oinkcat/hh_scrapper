import React from 'react';
import { Filters } from './components/Filters';
import { VacancyList } from './components/VacancyList';
import { TopScroller } from './components/TopScroller';

export default class App extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            error: null,
            list: [],
            isOfflineData: false,
            filterMode: Filters.FILTERS_MODE_ACTIVE
        };
    }

    vacanciesLoaded(vacanciesList, isOffline) {
        this.setState({
            list: vacanciesList,
            isOfflineData: isOffline
        });
    }

    fetchErrorOccurred(reason) {
        this.setState({ error: reason });
    }

    filterModeChanged(newMode) {
        this.setState({ filterMode: newMode });
    }

    renderContent() {
        return (
            <React.Fragment>
                <Filters
                    list={this.state.list}
                    localFallback={this.state.isOfflineData}
                    onModeChanged={this.filterModeChanged.bind(this)}
                />
                <VacancyList
                    onLoaded={this.vacanciesLoaded.bind(this)}
                    onError={this.fetchErrorOccurred.bind(this)}
                    mode={this.state.filterMode}
                />
                <TopScroller />
            </React.Fragment>
        );
    }

    renderError() {
        return (
            <h4>
                Ошибка получения данных: {this.state.error.toString()}!
            </h4>
        );
    }

    render() {
        return this.state.error ? this.renderError() : this.renderContent();
    }
}
