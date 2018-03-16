import React, { Component } from "react";
import ReactDOM from "react-dom";
import Team from "./teams";

class TeamLayout extends Component {

    constructor(props) {
        super(props);

        this.state = Object.create({
            teams: []
         });
    }

    componentWillMount = () => {
        $.ajax({
            type: "POST",
            url: "http://localhost:49649/AdminTeams/GetAllTeams/",
            dataType:"JSON",
            contentType:"applicatio/json; charset=utf-8",
        
        }).done(response => this.setState({ teams: response }));
        
    }

    render() {
        return (
            <Team teams = { this.state.teams } />
        )
    }
}

ReactDOM.render(<TeamLayout />, document.getElementById("teams-index"));