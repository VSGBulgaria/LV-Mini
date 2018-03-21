import React, { Component } from "react";
import Button from "react-bootstrap/lib/Button";

export default class User extends Component {
    onClick = () => {
        console.log("click me");
    }
    render() {
        return(
            <div>
                <Button onClick={this.onClick.bind(this)}>Default</Button>
                <h3>Hello from React</h3>
            </div>
        );
    }
}