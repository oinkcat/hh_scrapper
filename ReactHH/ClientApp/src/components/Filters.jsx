import React from 'react';

export class Filters extends React.Component {

    static FILTERS_MODE_ALL = 0;
    static FILTERS_MODE_ACTIVE = 1;
    static FILTERS_MODE_INACTIVE = 2;

    static getDerivedStateFromProps(props, state) {
        return { stats: [0, 0, 0] };
    }
    
    constructor(props) {
        super(props);

        this.state = {
            initialized: false,
            mode: Filters.FILTERS_MODE_ACTIVE,
            stats: [0, 0, 0]
        };
    }

    componentDidUpdate() {
        if (!this.state.initialized) {
            this.showStats(...this.getStats(this.props.list));
        }
    }

    showStats(total, active) {
        this.setState({
            initialized: true,
            stats: [total, active, total - active]
        });
    }

    getStats(vacancies) {
        const numActive = vacancies.filter(v => v.isActive).length;
        return [vacancies.length, numActive];
    }

    modeChanged(index) {
        this.setState({ mode: index });
        this.props.onModeChanged(index);
    }

    render() {
        const categories = ['Все', 'Активные', 'Закрытые'];

        return this.state.initialized && (
            <div className="filters-block">
                <ul>
                    {categories.map((c, i) => {
                        const active = i === this.state.mode;

                        return (
                            <li key={i}>
                                <label>
                                    <input onChange={this.modeChanged.bind(this, i)}
                                        checked={active}
                                        name="type"
                                        type="radio"
                                    />
                                    <span>{c} ({this.state.stats[i]})</span>
                                </label>
                            </li>
                        );
                    })}
                </ul>

                {this.props.localFallback && (
                    <span className="fall-back">
                        Оффлайн!
                    </span>
                )}
            </div>
        );
    }
}
