import React, { Component } from "react";
import ReactDOM from "react-dom";
import User from "./users";
import Pagenation from "./users";



ReactDOM.render(<User/>, document.getElementById("user-index"));

$.ajax({url: "http://localhost:49649/AdminUsers/UsersAsync/"}).done(response => {
    debugger;
});

