import React from 'react';
import { Col, Row } from 'reactstrap';

const Header: React.FC = () => {
  return (
    <Row className="mt-4">
      <Col lg="12" className="d-flex justify-content-between">
        <h3>
          Prova Salutem
        </h3>
        
        <span>
          Otávio Freitas
        </span>
      </Col>
    </Row>
  );
}

export default Header;