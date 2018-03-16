import React, { Component } from "react";
import Button from "react-bootstrap/lib/Button";
import Table from "react-bootstrap/lib/Table";
import { Grid, Row, Col } from 'react-bootstrap';

export default class User extends Component {
    onUserClick = users => {
        alert(`Edit ${users.username}`);
    }

    onUserDeleteClick = users =>{
        alert(`Delete ${users.username}`);
    }

    renderRow = users => {
        return users.map( (t, i) => {
            return (
                <tr key={i}>
                    <td>{t.username + ""}</td>
                    <td>{t.firstName}</td>
                    <td>{t.lastName}</td>
                    <td>{t.email}</td>
                    <td>
                        < Button onClick={ e => this.onUserClick(t) }>Edit</Button>&nbsp;
                        < Button onClick={ e => this.onUserDeleteClick(t) }>Delete</Button>
                    </td>
                </tr>
            )
        })
    }

    render() {
        return (
            <div className="form-horizontal well">
                <div className="row">
                    <section className="content">
                        <div className="col-md-8 col-md-offset-2">
                            <div className="panel-body">
                                <div className="user-container">
                                    <div>
                                        <Grid>
                                            <Row className="show-grid">
                                                <Col xs={12} md={8}>
                                                    <Table responsive triped bordered condensed hover>
                                                        <thead>
                                                            <tr>
                                                                <th>Username</th>
                                                                <th>First Name</th>
                                                                <th>Last Name</th>
                                                                <th>E-Mail</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                             { this.renderRow(this.props.users) }
                                                        </tbody>
                                                    </Table>
                                                </Col>
                                            </Row>
                                        </Grid>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        )
    }
}