import React from 'react';
import Routes from './routes';

const App: React.FC = () => <Routes />;

export default App;

/*import { useState, MouseEvent, useEffect } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import api from "./services/api";
import React from "react";
import { Alert, Button, Input } from "reactstrap";

interface Cliente {
  idCliente: number;
  cnpj: string;
  razaoSocial: string;
  latitude: string;
  longitude: string;
}

function GetCliente() {
  const [dadosCliente, setDadosCliente] = useState<Cliente[]>([]);

  useEffect(() => {
    api.get("cliente/all").then((response) => {
      setDadosCliente(response.data);
    });
  }, []);

  return (
    <div>
      {dadosCliente.map((cliente) => {
        return (
          <div key={cliente.idCliente}>
            <p>{cliente.cnpj}</p>
            <p>{cliente.razaoSocial}</p>
            <p>{cliente.latitude}</p>
            <p>{cliente.longitude}</p>
          </div>
        );
      })}
    </div>
  );
}

function App() {
  const [idCliente, setIdCliente] = useState<number>(0);
  const [cnpj, setCnpj] = useState<string>("");
  const [razaoSocial, setRazaoSocial] = useState<string>("");
  const [latitude, setLatitude] = useState<string>("");
  const [longitude, setLongitude] = useState<string>("");

  async function handleSubmit(event: MouseEvent) {
    event.preventDefault(); // previne o comportamento padrão do HTML

    try {
      const data: Cliente = {
        idCliente: idCliente,
        cnpj: cnpj,
        razaoSocial: razaoSocial,
        latitude: latitude,
        longitude: longitude,
      };

      await api.post("cliente", data);

      <div>
        <Alert color="success">Cadastrado com sucesso!</Alert>;
      </div>;

      setIdCliente(0);
      setCnpj("");
      setRazaoSocial("");
      setLatitude("");
      setLongitude("");
    } catch {
      <Alert color="danger">Erro ao cadastrar!</Alert>;
    }
  }

  return (
    <div className="App">
      <form style={{ marginTop: 20, marginLeft: 20 }}>
        <div>
          <label htmlFor="idCliente">ID do Cliente</label>
          <input
            style={{ marginLeft: 10 }}
            id="idCliente"
            type="number"
            placeholder="Digite o ID"
            onChange={(e) => setIdCliente(parseInt(e.target.value))}
          />

          <label style={{ marginLeft: 20 }} htmlFor="cnpj">
            CNPJ
          </label>
          <input
            style={{ marginLeft: 10 }}
            id="cnpj"
            type="text"
            placeholder="Digite o CNPJ"
            onChange={(e) => setCnpj(e.target.value)}
          />
        </div>

        <div style={{ marginTop: 20 }}>
          <label htmlFor="razaoSocial">Razão Social</label>
          <input
            style={{ marginLeft: 10 }}
            id="razaoSocial"
            type="text"
            placeholder="Digite a razão social"
            onChange={(e) => setRazaoSocial(e.target.value)}
          />

          <label style={{ marginLeft: 20 }} htmlFor="latitude">
            Latitude
          </label>
          <input
            style={{ marginLeft: 10 }}
            id="latitude"
            type="text"
            placeholder="Digite a latitude"
            onChange={(e) => setLatitude(e.target.value)}
          />
        </div>

        <div style={{ marginTop: 20 }}>
          <label htmlFor="longitude">Longitude</label>
          <input style={{ marginLeft: 10 }}
            id="longitude"
            type="text"
            placeholder="Digite a longitude"
            onChange={(e) => setLongitude(e.target.value)}
          />

          <Button style={{ marginLeft: 20 }} color="success" type="submit" onClick={handleSubmit}>
            Cadastrar
          </Button>
        </div>
      </form>
    </div>
  );
}

export default App;*/
