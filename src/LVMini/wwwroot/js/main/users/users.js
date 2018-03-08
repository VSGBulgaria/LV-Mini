import React, { Component } from "react";
import Button from "react-bootstrap/lib/Button";
import Table from "react-bootstrap/lib/Table";
import Pagination from 'react-bootstrap/lib/Pagination';


export default class User extends Component {
    onClick = () => {
        alert('Delete?');
    }

    togglePopup() {
        this.setState({
            showPopup: !this.state.showPopup
        });
    }
//pagination
constructor(props) {
    super(props);
    this.state = {
      activePage: 1,
      itemsCountPerPage:1,
      pageRangeDisplayed:1
    };
  }
 
  handlePageChange(pageNumber) {
    console.log(`active page is ${pageNumber}`);
    this.setState({activePage: pageNumber});
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
                                        <Table responsive triped bordered condensed hover>
                                            <thead>
                                                <tr>
                                                    <th>#</th>
                                                    <th>Username</th>
                                                    <th>First Name</th>
                                                    <th>E-Mail</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Mark</td>
                                                    <td>Otto</td>
                                                    <td>@mdo</td>
                                                    <td>  <Button onClick={this.togglePopup.bind(this)}>Edit</Button>&nbsp;
                                                    <Button onClick= {this.onClick.bind(this)} >Delete</Button></td>
                                                </tr>
                                                <tr>
                                                    <td>2</td>
                                                    <td>Jacob</td>
                                                    <td>Thornton</td>
                                                    <td>@fat</td>
                                                    <td> <Button onClick={this.onClick.bind(this)}>Edit</Button>&nbsp;
                                                    <Button onClick={this.onClick.bind(this)} >Delete</Button></td>
                                                </tr>
                                                <tr>
                                                    <td>3</td>
                                                    <td >Larry the Bird</td>
                                                    <td></td>
                                                    <td>@twitter</td>
                                                   <td> <Button onClick={this.onClick.bind(this)}>Edit</Button>&nbsp;
                                                    <Button onClick={this.onClick.bind(this)} >Delete</Button></td>
                                                </tr>
                                            </tbody>
                                        </Table>
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
