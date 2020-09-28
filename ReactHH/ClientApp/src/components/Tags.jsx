import React from 'react';

export function Tags(props) {
    return (
        <ul className="tag-list">
            {props.list.map(tag => <li key={tag}>{tag}</li>)}
        </ul>
    );
}