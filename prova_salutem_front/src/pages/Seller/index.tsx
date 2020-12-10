import api from "../../services/api";
import Header from "../../components/Header";
import React, { ChangeEvent, FormEvent, useState } from "react";
import SellerPageBar from "../../components/PageBars/SellerPageBar";
import {  Card, CardBody, CardFooter, CardHeader, Col, Container, Form, FormGroup, Modal, Row, Table, Label, Input, Button} from "reactstrap";

//cria interface Vendedor
interface Vendedor {
    idVendedor: number;
  cpf: string;
  nomeVendedor: string;
  latitude: string;
  longitude: string;
}

//cria um novo objeot Vendedor
const blankVendedor: Vendedor = {
  idVendedor: 0,
  cpf: '',
  nomeVendedor: '',
  latitude: '',
  longitude: '',
};

//busca vendedores cadastrados assim que a página carrega e mostra na tela
const Seller: React.FC = () => {
  const [sellers, setSellers] = useState<Vendedor[]>(() => {
    api.get('vendedor/all').then((response) => {
      setSellers(response.data);
    });

    return [];
  });

  // Modal de novo vendedor
  const [newSeller, setNewSeller] = useState<Vendedor>(blankVendedor);
  const [sellerModalIsVisible, setSellerModalIsVisible] = useState(false);
  const toggleSellerModal = () => setSellerModalIsVisible(!sellerModalIsVisible);
  const closeSellerModal = () => {
    setNewSeller(blankVendedor);
    setSellerModalIsVisible(false);
  }

  //verificador para saber se é atualização de vendedor
  const [currentSeller, setCurrentSeller] = useState<Vendedor>(blankVendedor);
  const [isEditing, setIsEditing] = useState(false);
  const editRow = (vendedor: Vendedor) => {
    setCurrentSeller(vendedor);
    setIsEditing(true);
    setSellerModalIsVisible(true);
  }

  //Funções de handle
  const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.currentTarget;

    if (isEditing){
      setCurrentSeller({ ...currentSeller, [name]: value});
    } else {
      setNewSeller({ ...newSeller, [name]: value});
    }
  }

  //cadastra novo vendedor
  const handleSubmitNewSeller = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    try {
      newSeller.idVendedor = parseInt(newSeller.idVendedor.toString());
      await api.post('vendedor', newSeller);

      alert("Vendedor cadastrado com sucesso!");
      closeSellerModal();

      window.location.reload();
    } catch (error) {
      console.log(error);

      alert("Ocorreu um erro ao cadastrar!");
    }
  }

  //atualiza vendedor
  const handleSubmitSellerUpdate = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    try {
      await api.put('vendedor', currentSeller);

      alert('Vendedor atualizado com sucesso!');

      window.location.reload();
    } catch (error) {
      console.log(error);

      alert('Ocorreu um erro ao atualizar!');
    }
  }

  //deleta vendedor
  const handleDeleteSeller = async (idVendedor: number) => {
    try {
      await api.delete(`vendedor?idVendedor=${idVendedor}`);

      alert("Vendedor deletado com sucesso!");

      window.location.reload();
    } catch (error) {
      console.log(error);

      alert("Ocorreu um erro ao deletar!");
    }
  }

  return (
    <>
      <Modal isOpen={sellerModalIsVisible} toggle={toggleSellerModal} size="lg">
        <Form onSubmit={(e) => { // verifica se é edição de cadastro
          console.log({ isEditing, newSeller, currentSeller });
          isEditing 
            ? handleSubmitSellerUpdate(e) 
            : handleSubmitNewSeller(e)
          }}>
          <Card>
            <CardHeader>Adicionar novo vendedor</CardHeader> 
            <CardBody>
              <Row>
                <Col lg="2">
                  <FormGroup>
                    <Label htmlFor="idVendedor">ID Vendedor</Label> // c
                    <Input 
                      type="number" 
                      placeholder="ID Vendedor" 
                      name="idVendedor" 
                      value={isEditing ? currentSeller.idVendedor : newSeller.idVendedor} 
                      onChange={handleInputChange} 
                    />
                  </FormGroup>
                </Col>
                <Col lg="4">
                  <FormGroup>
                    <Label htmlFor="cpf">CPF</Label>
                    <Input 
                      type="text" 
                      placeholder="Digite o CPF..." 
                      name="cpf" 
                      value={isEditing ? currentSeller.cpf : newSeller.cpf} 
                      onChange={handleInputChange} 
                    />
                  </FormGroup>
                </Col>
                <Col lg="6">
                  <FormGroup>
                    <Label htmlFor="nomeVendedor">Nome do Vendedor</Label>
                    <Input 
                      type="text" 
                      placeholder="Digite o nome do vendedor..." 
                      name="nomeVendedor" 
                      value={isEditing ? currentSeller.nomeVendedor : newSeller.nomeVendedor} 
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
                      value={isEditing ? currentSeller.latitude : newSeller.latitude} 
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
                      value={isEditing ? currentSeller.longitude : newSeller.longitude} 
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
                  <Button type="button" color="danger" onClick={closeSellerModal}>Cancelar</Button>
                </Col>
              </Row>
            </CardFooter>
          </Card>
        </Form>
      </Modal>

      <Container>
        <Header />
        <SellerPageBar newSellerFunction={() => {
          setIsEditing(false);
          toggleSellerModal();
        }} />
        <Row className="mt-2">
          <Col lg="12">
            <Card className="mt-4">
              <CardBody className="p-0">
                <Table responsive small hover>
                  <thead className="thead-light">
                    <tr>
                      <th>ID Vendedor</th>
                      <th>CPF</th>
                      <th>Nome do Vendedor</th>
                      <th>Latitude</th>
                      <th>Longitude</th>
                      <th> </th>
                    </tr>
                  </thead>
                  <tbody>
                    {sellers.map((vendedor, index) => (
                      <tr key={`${vendedor.nomeVendedor}&${index}`}>
                        <td>{vendedor.idVendedor}</td>
                        <td>{vendedor.cpf}</td>
                        <td>{vendedor.nomeVendedor}</td>
                        <td>{vendedor.latitude}</td>
                        <td>{vendedor.longitude}</td>
                        <td>
                          <div>
                          <Button type="button" color="info" onClick={(e) => editRow(vendedor)}>
                            Editar
                          </Button>
                          <Button className="ml-2" type="button" color="danger" onClick={(e) => handleDeleteSeller(vendedor.idVendedor)}>
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

export default Seller;
