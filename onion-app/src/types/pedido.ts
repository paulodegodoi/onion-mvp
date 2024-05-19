import { Cliente } from "./cliente"
import { Produto } from "./produto"

export type Pedido = {
	id: number
	numeroPedido: number
	cep: string
	uf: string
	valorFinal: number
	produto: Produto
	cliente: Cliente
	dataCriacao: string
	dataEntrega: string
}
