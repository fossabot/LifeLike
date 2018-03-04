import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import LogContainer from './Components/ListContainer';


export class LogLayout extends React.Component<RouteComponentProps<{}>, {}> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);
    }


    public render() {
        let contents = <LogContainer/>;
        return <section className="resume-section">
            <div className="jumbotron">
                <p className="lead">LOGS</p>
            </div>
            { contents }
        </section>;
    }
}
