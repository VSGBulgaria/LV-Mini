import React, {Component} from "react";
import Button from "react-bootstrap/lib/Button";
import Table from "react-bootstrap/lib/Table";
import { Grid, Row, Col } from 'react-bootstrap';

export default class Team extends Component{
    onTeamClick = team => {
        alert(`I just clicked on ${team.teamName}`);
    }
    renderRow = teams => {
        return teams.map( (t, i) => {
            return (
                <tr key={i}>
                    <td>{t.isActive + ""}</td>
                    <td>{t.teamName}</td>
                    <td>< Button onClick={ e => this.onTeamClick(t) }>View</Button></td>
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
                                                            <th>Is Active</th>
                                                            <th>Team Name</th>
                                                            <th>    </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        { this.renderRow(this.props.teams) }
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