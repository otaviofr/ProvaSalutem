import React from 'react';
import { useHistory } from 'react-router-dom';
import { Button, Col, Container, Row } from 'reactstrap';

import Header from '../../components/Header';

const Dashboard: React.FC = () => {
  const history = useHistory();
  return (
    <Container>
      <Header />
      <Row className="mt-4">
        <Col lg="3">
          <Button type="button" color="info" onClick={() => {
            history.push('/customer');
          }}>
            Clientes
          </Button>
        </Col>
        <Col lg="4">
          <Button type="button" color="info" onClick={() => {
            history.push('/seller');
          }}>
            Vendedores
          </Button>
        </Col>
      </Row>
    </Container>
  )
}

export default Dashboard;