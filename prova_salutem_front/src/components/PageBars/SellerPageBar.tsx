import React from 'react';
import { Button, Col, Row } from 'reactstrap';
import { useHistory } from 'react-router-dom';

interface ISellerPageBarProps {
  newSellerFunction(): void;
}

const SellerPageBar: React.FC<ISellerPageBarProps> = ({ newSellerFunction }) => {
  const history = useHistory();

  return (
    <Row className="mt-4">
      <Col lg="12" className="d-flex justify-content-between">
        <Button type="button" color="success" onClick={newSellerFunction}>Adicionar</Button>
        
        <Button type="button" color="secondary" onClick={() => {
          history.push('/');
        }}>
          Ir para o início
        </Button>
      </Col>
    </Row>
  )
}

export default SellerPageBar;