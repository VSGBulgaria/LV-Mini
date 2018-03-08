import React, {Component} from "react";
import ReactDOM from "react-dom";
import Team from "./teams";

ReactDOM.render(<Team/>, document.getElementById("teams-index"));

$.ajax({url: "http://localhost:49649/AdminTeams/GetAllTeams/"}).done(response => {
    debugger;
});