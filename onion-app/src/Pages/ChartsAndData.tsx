import { useContext, useEffect } from "react"
import { Button } from "../Components/Button"
import { Chart } from "../Components/Chart"
import { OrdersList } from "../Components/OrdersList"
import { OrdersDataContext } from "../contexts/OrdersDataContext"
import { ChartType } from "../enums/chartType"
import { Link, useNavigate } from "react-router-dom"
import { Pedido } from "../types/pedido"

export function ChartsAndData() {
	const { ordersData, setOrdersData } = useContext(OrdersDataContext)
	const stringOrdersDataStorage = localStorage.getItem("ordersDataKey")
	const navigate = useNavigate()

	useEffect(() => {
		// verificar orderData pelo context
		if (ordersData != null) {
			const stringOrders = JSON.stringify(ordersData)
			localStorage.setItem("ordersDataKey", stringOrders)
		}
		// se ordersData do context for null, tenta utilizar o orderData do localStorage
		else if (stringOrdersDataStorage != null && ordersData == null) {
			const ordersDataStorage = JSON.parse(stringOrdersDataStorage) as Pedido[]
			setOrdersData(ordersDataStorage)
		} else navigate("/")
	}, [ordersData, navigate])

	return (
		<>
			{ordersData != null && (
				<div className="flex flex-col items-center py-4">
					{/* <Link to="/">Gerar novos dados</Link> */}
					<Link to="/">
						<Button
							bgColor="yellow"
							text="Gerar novos dados"
							type="button"
							// onClick={() => setOrdersData(null)}
						/>
					</Link>
					<div className="w-screen flex flex-col items-center lg:flex-row md:justify-around">
						<Chart ordersData={ordersData} chartType={ChartType.Regiao} />
						<Chart ordersData={ordersData} chartType={ChartType.Produto} />
					</div>
					<OrdersList ordersData={ordersData} />
				</div>
			)}
		</>
	)
}
