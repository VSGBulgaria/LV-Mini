import React, {Component} from "react";
import Table from "react-bootstrap/lib/Table";
import Grid from "react-boostrap/lib/Grid";

export default class Team extends Component{
    render(){
        return(
            <div>
                <Grid>
                    <Row className="show-grid">
                        <Col xs={12} md={8}>
                        <code>&lt;{'Col xs={12} md={8}'} /">">">&gt;</code>
                        </Col>
                        <Col xs={6} md={4}>
                        <code>&lt;{'Col xs={6} md={4}'} /">">">&gt;</code>
                        </Col>
                    </Row>
                </Grid>;
            </div>
        )
    }
}