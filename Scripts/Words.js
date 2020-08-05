class WordBox extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: [] ,types : []};
        this.handleWordSubmit = this.handleWordSubmit.bind(this);
    }
    loadCommentsFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
    }

    
    componentDidMount() {
        this.loadCommentsFromServer();      
        window.setInterval(
            () => this.loadCommentsFromServer(),
            this.props.pollInterval,
         
        );
       
    }
    handleWordSubmit(word) {
        const data = new FormData();
        data.append('french', word.french);
        data.append('vietnam', word.vietnam);
        data.append('type', word.type);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = () => this.loadCommentsFromServer();
        xhr.send(data);
    }
    render() {
        return (
            <div className="wordbox">
                <br />
                <WordForm onWordSubmit={this.handleWordSubmit} />
                <br />
                <div className="container-fluid wordgrid">
                    <WorldTitle />
                    <WordList data={this.state.data} />
                </div>
            </div>
        );
    }
}

class WorldTitle extends React.Component {
    render() {
        return (
            <div className="row text-left wordtitle">
                <div className="col-sm-5 col-xs-4 font-weight-bold text-dark">Français</div>
                <div className="col-sm-5 col-xs-5 text-danger">Vietnamien</div>
                <div className="col-sm-1 col-xs-1 text-success">Type</div>                
            </div>
        );
    }
}


class Word extends React.Component {
    render() {        
        return (
            <div className="row text-left word">
                <div className="col-sm-5 col-xs-4 text-dark font-weight-bold">{this.props.french}</div>
                <div className="col-sm-5 col-xs-5 text-danger">{this.props.vietnam}</div>
                <div className="col-sm-1 col-xs-1 text-success">{this.props.type}</div> 
            </div>
        );
    }
}

class WordList extends React.Component {
    render() {
        const wordtNodes = this.props.data.map(word => (
            <Word french={word.French} vietnam={word.Vietnam} type={word.Type} key={word.Id} />
        ));
        return <div className="WordList">{wordtNodes}</div>;
    }
}

class WordForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { french: '', vietnam: '', type: '',types:[] };
        this.handleFrenchChange = this.handleFrenchChange.bind(this);
        this.handleVietnamChange = this.handleVietnamChange.bind(this);
        this.handleTypeChange = this.handleTypeChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    loadTypesFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', '/api/WordsNg/GetTypes', true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ types: data });
        };
        xhr.send();
    }

    handleFrenchChange(e) {
        this.setState({ french: e.target.value });
    }
    handleVietnamChange(e) {
        this.setState({ vietnam: e.target.value });
    }
    handleTypeChange(e) {
        this.setState({ type: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        const french = this.state.french.trim();
        const vietnam = this.state.vietnam.trim();
        const type = this.state.type.trim();
        if (!french || !vietnam || !type) {
            return;
        }
        this.props.onWordSubmit({ french: french, vietnam: vietnam, type: type });
        this.setState({ french: '', vietnam: '', type: ''});
    }

    componentDidMount() {
        this.loadTypesFromServer();
    }
    render() {
        //console.log(this.state.types);
        return (
            <div className="row wordform">
                <form className="WordForm" onSubmit={this.handleSubmit}>
                    <input className="col-lg-5 col-md-5 col-sm-5 col-xs-3 text-dark input-sm rounded" type="text" placeholder="Français" value={this.state.french} onChange={this.handleFrenchChange} />
                    <input className="col-lg-6 col-md-6 col-sm-6 col-xs-4 text-danger input-sm" type="text" placeholder="Vietnamien" value={this.state.vietnam} onChange={this.handleVietnamChange} />
                    
                    <select className="col-lg-1 col-md-1 col-sm-1 col-sm-1 col-xs-2 text-success input-sm" ng-if="!this.state.types" value={this.state.type} onChange={this.handleTypeChange}>
                        <option>-Select type-</option>
                        {
                            this.state.types &&
                            this.state.types.map((h) =>
                                (<option key={h} value={h}>{h}</option>))
                        }
                    </select>
                    <input className="btn btn-info btn-sm col-lg-1 col-md-1 col-sm-1 col-xs-2" type="submit" value="Ạjouter" />
                </form>
                
            </div>
        );
    }
}
ReactDOM.render(<WordBox url="/Home/Words" submitUrl="/Home/Create" pollInterval={300000} />, document.getElementById('content'));