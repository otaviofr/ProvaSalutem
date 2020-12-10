import { useState, MouseEvent, useEffect } from "react";
import api from "./services/api";

interface Human {
  nome: string;
  tamanhoDaRola: string;
}

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

      alert("Enviado com sucesso");

      setIdCliente(0);
      setCnpj("");
      setRazaoSocial("");
      setLatitude("");
      setLongitude("");
    } catch {
      alert("Ocorreu um erro ao enviar");
    }
  }

  return (
    <div className="App">
      <form>
        <label htmlFor="idCliente">ID do Cliente</label>
        <input
          id="idCliente"
          type="number"
          placeholder="Digite o ID"
          onChange={(e) => setIdCliente(parseInt(e.target.value))}
        />

        <label htmlFor="cnpj">CNPJ</label>
        <input
          id="cnpj"
          type="text"
          placeholder="Digite o CNPJ"
          onChange={(e) => setCnpj(e.target.value)}
        />

        <label htmlFor="razaoSocial">Razão Social</label>
        <input
          id="razaoSocial"
          type="text"
          placeholder="Digite a razão social"
          onChange={(e) => setRazaoSocial(e.target.value)}
        />

        <label htmlFor="latitude">Latitude</label>
        <input
          id="latitude"
          type="text"
          placeholder="Digite a latitude"
          onChange={(e) => setLatitude(e.target.value)}
        />

        <label htmlFor="longitude">Longitude</label>
        <input
          id="longitude"
          type="text"
          placeholder="Digite a longitude"
          onChange={(e) => setLongitude(e.target.value)}
        />

        <button type="submit" onClick={handleSubmit}>
          Enviar
        </button>
      </form>
    </div>
  );
}

export default App;
