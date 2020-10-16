import React from 'react';

export function Tags(props) {
    return (
        <ul className="tag-list">
            {props.list.map((tag, i) => <li key={i}>{tag}</li>)}
        </ul>
    );
}