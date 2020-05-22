class WordBox extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: [] };
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
            <div className="WordBox">
                <WordForm onWordSubmit={this.handleWordSubmit} />
                <WorldTitle />
                <WordList data={this.state.data} />    
            </div>
        );
    }
}

class WorldTitle extends React.Component {
    render() {
        return (
            <h4 className="row text-left  font-weight-bold">
                <div className="col-sm-5 text-dark">Français</div>
                <div className="col-sm-6 text-danger">Vietnamien</div>
                <div className="col-sm-1 text-success">Type</div>
            </h4>
        );
    }
}


class Word extends React.Component {
    render() {
        return (
            <div className="row text-left">
                <div className="col-sm-5 text-dark font-weight-bold">{this.props.french}</div>
                <div className="col-sm-6 text-danger">{this.props.vietnam}</div>
                <div className="col-sm-1 text-success">{this.props.type}</div>
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
        this.state = { french: '', vietnam: '', type: '' };
        this.handleFrenchChange = this.handleFrenchChange.bind(this);
        this.handleVietnamChange = this.handleVietnamChange.bind(this);
        this.handleTypeChange = this.handleTypeChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
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
        this.setState({ french: '', vietnam: '', type: '' });
    }
    render() {
        return (
            <div className="row wordform">
                <form className="WordForm" onSubmit={this.handleSubmit}>
                    <input className="form-control text-dark col-md-4" type="text" placeholder="Mot français" value={this.state.french} onChange={this.handleFrenchChange} />       
                    <input className="form-control col-md-4 text-danger" type="text" placeholder="Mot vietnamien" value={this.state.vietnam} onChange={this.handleVietnamChange} />
                    <input className="form-control col-md-2 text-success" type="text" placeholder="Type" value={this.state.type} onChange={this.handleTypeChange} />
                    <input className="btn btn-primary col-md-1" type="submit" value="Ạouter" />
                </form>
            </div>
        );
    }
}
ReactDOM.render(<WordBox url="/Home/Words" submitUrl="/Home/Create" pollInterval={300000} />, document.getElementById('content'));