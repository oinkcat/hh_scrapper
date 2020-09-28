import React from 'react';

export class TopScroller extends React.Component {

    constructor(props) {
        super(props);

        this.state = { visible: false };
    }

    componentDidMount() {
        const HEADER_HEIGHT = 50;

        document.addEventListener("scroll", () => {
            if (document.documentElement.scrollTop >= HEADER_HEIGHT) {
                if (!this.state.visible) {
                    this.setState({ visible: true });
                }
            } else {
                if (this.state.visible) {
                    this.setState({ visible: false });
                }
            }
        });
    }

    scrollToTop() {
        document.documentElement.scrollTop = 0;
    }

    render() {
        let className = 'top-scroller';
        if (this.state.visible) {
            className += ' visible';
        }

        return (
            <div title="Scroll to top"
                onClick={this.scrollToTop}
                className={className}>
                ^
            </div>
        );
    }
}