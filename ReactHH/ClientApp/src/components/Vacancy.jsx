import React from 'react';
import { Tags } from './Tags';

function showDaysActive(info) {
    let lastUpdateTs = new Date(info.lastUpdateDate);
    let firstFoundTs = new Date(info.firstFoundDate);
    let deltaTicks = lastUpdateTs - firstFoundTs;
    let activeDays = Math.floor(deltaTicks / 1000 / 60 / 60 / 24);

    return (
        <div className="close-days">
            Время закрытия вакансии (дней): {activeDays}
        </div>
    );
}

// A vacancy information
export function Vacancy(props) {
    let itemClass = 'vacancy-item';
    if (!props.info.isActive) {
        itemClass += ' inactive';
    }

    return (
        <div className={itemClass}>
            <p>
                <a href={props.info.url} target="_blank">
                    {props.info.title}
                </a>
            </p>
            <div className="align-right">{props.info.salary}</div>
            <p>{props.info.companyName}</p>
            <div className="clear" />

            <Tags list={props.info.tags} />
            <div className="clear" />

            {!props.info.isActive && showDaysActive(props.info)}
        </div>
    );
}