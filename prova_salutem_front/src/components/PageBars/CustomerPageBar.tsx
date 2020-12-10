import React from 'react';
import { Button, Col, Row } from 'reactstrap';
import { useHistory } from 'react-router-dom';

interface ICustomerPageBarProps {
  newCustomerFunction(): void;
}

const CustomerPageBar: React.FC<ICustomerPageBarProps> = ({ newCustomerFunction }) => {
  const history = useHistory();

  return (
    <Row className="mt-4">
      <Col lg="12" className="d-flex justify-content-between">
        <Button type="button" color="success" onClick={newCustomerFunction}>Adicionar</Button>
        
        <Button type="button" color="secondary" onClick={() => {
          history.push('/');
        }}>
          Ir para o início
        </Button>
      </Col>
    </Row>
  )
}

export default CustomerPageBar;