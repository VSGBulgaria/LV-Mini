import React, { Component } from "react";
import ReactDOM from "react-dom";
import User from "./users";

class UserLayout extends Component{

    constructor(props){
        super(props);

        this.state = Object.create({
            users:[]
        });
    }

    componentWillMount = () =>{
        $.ajax({
            type:"POST",
            url: "http://localhost:49649/AdminUsers/UsersAsync/",
            dataType:"JSON",
            contentType:"applicatio/json; charset=utf-8",

        }).done(response => this.setState({ users:response}));

    }

    render(){
        return(
            <User users = { this.state.users }/>
        )
    }
}

ReactDOM.render(<UserLayout />, document.getElementById("user-index"));