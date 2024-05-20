import { ReactNode, createContext, useState } from "react"
import { Pedido } from "../types/pedido"

type OrdersDataContextProps = {
	children: ReactNode
}

type OrdersDataContextType = {
	ordersData: Pedido[] | null
	setOrdersData: (newState: Pedido[]) => void
}

const initialValue = {
	ordersData: null,
	setOrdersData: () => {},
}

export const OrdersDataContext = createContext<OrdersDataContextType>(initialValue)

export const OrdersDataContextProvider = ({ children }: OrdersDataContextProps) => {
	const [ordersData, setOrdersData] = useState<Pedido[] | null>(initialValue.ordersData)

	return (
		<OrdersDataContext.Provider value={{ ordersData, setOrdersData }}>
			{children}
		</OrdersDataContext.Provider>
	)
}
