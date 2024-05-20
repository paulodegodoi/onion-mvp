import { formatToBrazilianDate, formatToCurrency } from "../functions/myFunctions"
import { Pedido } from "../types/pedido"

interface IOrdersList {
	ordersData: Pedido[]
}
export function OrdersList({ ordersData }: IOrdersList) {
	return (
		<div className="px-2 w-screen text-center">
			<h3 className="text-2xl">Lista de pedidos</h3>
			<table className="table-auto w-full m-auto border-2 border-blue-900 my-4 text-xs md:text-xl md:w-10/12">
				<thead className="bg-blue-700 text-white">
					<tr>
						<th>Cliente</th>
						<th>Produto</th>
						<th>Valor total</th>
						<th>Data da entrega</th>
					</tr>
				</thead>
				<tbody>
					{ordersData.map((order, index) => (
						<tr
							key={order.id}
							className={`${index + 1 < ordersData.length && "border-b-2"}`}
						>
							<td>{order.cliente.razaoSocial.toUpperCase()}</td>
							<td>{order.produto.nome.toUpperCase()}</td>
							<td>{formatToCurrency(order.valorFinal)}</td>
							<td>{formatToBrazilianDate(order.dataEntrega)}</td>
						</tr>
					))}
				</tbody>
			</table>
		</div>
	)
}
