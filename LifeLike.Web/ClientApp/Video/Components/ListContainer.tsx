import * as React from 'react';
import ListView from '../../Components/VideoList/ListView';
import EmptyListView from '../../Components/EmptyList/EmptyListView';
import LoadingView from '../../Components/Loading/LoadingView';

import Item from '../../Models/MenuItem';

interface ListContainerState {
    loadingData: boolean,
    items: Item[]
}
interface ListContainerProps {}

class ListContainer extends React.Component<ListContainerProps, ListContainerState> {

    private paths = {
        getList: '/Api/Video/'
    };

    constructor(props: ListContainerProps) {
        super(props);

        this.state = {
            loadingData: true,
            items: []
        };
    }
    public componentDidMount() {
        fetch(this.paths.getList, {
            credentials: 'include' })
            .then((response) => {
                return response.text();
            })
            .then((data) => {
                this.setState({
                    items: JSON.parse(data),
                    loadingData: false
                });
            });
    }
    public render() {
        const hasProjects = this.state.items.length > 0;

        return (
            this.state.loadingData ?
                <LoadingView Title={"Videos"}/> :
                hasProjects ?
                    <ListView items= {this.state.items} /> :  <EmptyListView Title={"Videos"}/>
        )
    }
 
}

export default ListContainer;