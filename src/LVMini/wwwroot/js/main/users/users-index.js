import React, { Component } from "react";
import ReactDOM from "react-dom";
import User from "./users";
import Pagenation from "./users";



ReactDOM.render(<User/>, document.getElementById("user-index"));

$.ajax({
    type: "POST",
    url: "http://localhost:49649/AdminUsers/UsersAsync/",
    data:"{'id':'"+$('#username').val() +"'}",
    dataType:"JSON",
    contentType:"applicatio/json; charset=utf-8",
}).done(response => {
    // debugger;
});

