import React, { ChangeEvent, FormEvent, useState } from 'react';
import { Card, CardBody, CardFooter, CardHeader, Col, Container, Form, FormGroup, Modal, Row, Table, Label, Input, Button } from 'reactstrap';
import Header from '../../components/Header';
import CustomerPageBar from '../../components/PageBars/CustomerPageBar';
import api from '../../services/api';

//cria interface Cliente
interface Cliente {
  idCliente: number;
  cnpj: string;
  razaoSocial: string;
  latitude: string;
  longitude: string;
}

//cria um novo objeot Cliente
const blankCliente: Cliente = {
  idCliente: 0,
  cnpj: '',
  razaoSocial: '',
  latitude: '',
  longitude: '',
}

//busca clientes cadastrados assim que a página carrega e mostra na tela
const Customer: React.FC = () => {
  const [customers, setCustomers] = useState<Cliente[]>(() => {
    api.get('cliente/all').then((response) => {
      setCustomers(response.data);
    });

    return [];
  });

  // Modal de novo cliente
  const [newCustomer, setNewCustomer] = useState<Cliente>(blankCliente);
  const [customerModalIsVisible, setCustomerModalIsVisible] = useState(false);
  const toggleCustomerModal = () => setCustomerModalIsVisible(!customerModalIsVisible);
  const closeCustomerModal = () => {
    setNewCustomer(blankCliente);
    setCustomerModalIsVisible(false);
  }

  //verificador para saber se é atualização de cliente
  const [currentCustomer, setCurrentCustomer] = useState<Cliente>(blankCliente);
  const [isEditing, setIsEditing] = useState(false);
  const editRow = (cliente: Cliente) => {
    setCurrentCustomer(cliente);
    setIsEditing(true);
    setCustomerModalIsVisible(true);
  }

  //Funções de handle
  const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.currentTarget;

    if (isEditing){
      setCurrentCustomer({ ...currentCustomer, [name]: value});
    } else {
      setNewCustomer({ ...newCustomer, [name]: value});
    }
  }

  //cadastra novo cliente
  const handleSubmitNewCustomer = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    try {
      newCustomer.idCliente = parseInt(newCustomer.idCliente.toString());
      await api.post('cliente', newCustomer);

      alert("Cliente cadastrado com sucesso!");
      closeCustomerModal();

      window.location.reload();
    } catch (error) {
      console.log(error);

      alert("Ocorreu um erro ao cadastrar!");
    }
  }

  //atualiza cliente
  const handleSubmitCustomerUpdate = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    try {
      await api.put('cliente', currentCustomer);

      alert('Cliente atualizado com sucesso!');

      window.location.reload();
    } catch (error) {
      console.log(error);

      alert('Ocorreu um erro ao atualizar!');
    }
  }

  //deleta cliente
  const handleDeleteCustomer = async (idCliente: number) => {
    try {
      await api.delete(`cliente?idCliente=${idCliente}`);

      alert("Cliente deletado com sucesso!");

      window.location.reload();
    } catch (error) {
      console.log(error);

      alert("Ocorreu um erro ao deletar!");
    }
  }

  return (
    <>
      <Modal isOpen={customerModalIsVisible} toggle={toggleCustomerModal} size="lg">
        <Form onSubmit={(e) => { // verifica se é edição de cadastro
          console.log({ isEditing, newCustomer, currentCustomer });
          isEditing 
            ? handleSubmitCustomerUpdate(e) 
            : handleSubmitNewCustomer(e)
          }}>
          <Card>
            <CardHeader>Adicionar novo cliente</CardHeader>
            <CardBody>
              <Row>
                <Col lg="2">
                  <FormGroup>
                    <Label htmlFor="idCliente">ID Cliente</Label>
                    <Input 
                      type="number" 
                      placeholder="ID Cliente" 
                      name="idCliente" 
                      value={isEditing ? currentCustomer.idCliente : newCustomer.idCliente} 
                      onChange={handleInputChange} 
                    />
                  </FormGroup>
                </Col>
                <Col lg="4">
                  <FormGroup>
                    <Label htmlFor="cnpj">CNPJ</Label>
                    <Input 
                      type="text" 
                      placeholder="Digite o CNPJ..." 
                      name="cnpj" 
                      value={isEditing ? currentCustomer.cnpj : newCustomer.cnpj} 
                      onChange={handleInputChange} 
                    />
                  </FormGroup>
                </Col>
                <Col lg="6">
                  <FormGroup>
                    <Label htmlFor="razaoSocial">Razão Social</Label>
                    <Input 
                      type="text" 
                      placeholder="Digite a razão social..." 
                      name="razaoSocial" 
                      value={isEditing ? currentCustomer.razaoSocial : newCustomer.razaoSocial} 
                      onChange={handleInputChange} 
                    />
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <Col lg="6">
                  <FormGroup>
                    <Label htmlFor="latitude">Latitude</Label>
                    <Input 
                      type="text" 
                      placeholder="Digite a latitude..." 
                      name="latitude" 
                      value={isEditing ? currentCustomer.latitude : newCustomer.latitude} 
                      onChange={handleInputChange} 
                    />
                  </FormGroup>
                </Col>
                <Col lg="6">
                  <FormGroup>
                    <Label htmlFor="longitude">Longitude</Label>
                    <Input 
                      type="text" 
                      placeholder="Digite a longitude..." 
                      name="longitude" 
                      value={isEditing ? currentCustomer.longitude : newCustomer.longitude} 
                      onChange={handleInputChange} 
                    />
                  </FormGroup>
                </Col>
              </Row>
            </CardBody>
            <CardFooter>
              <Row>
                <Col lg="12" className="d-flex justify-content-between">
                  <Button type="submit" color="success">Confirmar</Button>
                  <Button type="button" color="danger" onClick={closeCustomerModal}>Cancelar</Button>
                </Col>
              </Row>
            </CardFooter>
          </Card>
        </Form>
      </Modal>

      <Container>
        <Header />
        <CustomerPageBar newCustomerFunction={() => {
          setIsEditing(false);
          toggleCustomerModal();
        }} />
        <Row className="mt-2">
          <Col lg="12">
            <Card className="mt-4">
              <CardBody className="p-0">
                <Table responsive small hover>
                  <thead className="thead-light">
                    <tr>
                      <th>ID Cliente</th>
                      <th>CNPJ</th>
                      <th>Razão Social</th>
                      <th>Latitude</th>
                      <th>Longitude</th>
                      <th> </th>
                    </tr>
                  </thead>
                  <tbody>
                    {/* AQUI DEVERÁ COLOCAR O ESTADO QUE CONTÉM DADOS DA API (customers) */}
                    {customers.map((cliente, index) => (
                      <tr key={`${cliente.razaoSocial}&${index}`}>
                        <td>{cliente.idCliente}</td>
                        <td>{cliente.cnpj}</td>
                        <td>{cliente.razaoSocial}</td>
                        <td>{cliente.latitude}</td>
                        <td>{cliente.longitude}</td>
                        <td>
                          <div>
                          <Button type="button" color="info" onClick={(e) => editRow(cliente)}>
                            Editar
                          </Button>
                          <Button className="ml-2" type="button" color="danger" onClick={(e) => handleDeleteCustomer(cliente.idCliente)}>
                            Excluir
                          </Button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </Table>
              </CardBody>
            </Card>
          </Col>
        </Row>
      </Container>
    </>
  )
}

export default Customer;