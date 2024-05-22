import { Route, HashRouter as Router, Routes } from "react-router-dom"
import { Home } from "./Pages/Home"
import { Layout } from "./Layout"
import { OrdersDataContextProvider } from "./contexts/OrdersDataContext"
import { ChartsAndData } from "./Pages/ChartsAndData"
import { Products } from "./Pages/Products"
import { EditProduct } from "./Pages/EditProduct"

function App() {
	return (
		<OrdersDataContextProvider>
			<Router>
				<Routes>
					<Route element={<Layout />}>
						<Route
							path="/"
							element={
								<OrdersDataContextProvider>
									<Home />
								</OrdersDataContextProvider>
							}
						/>
						<Route
							path="/dados-informativos"
							element={
								<OrdersDataContextProvider>
									<ChartsAndData />
								</OrdersDataContextProvider>
							}
						/>
						<Route
							path="/produtos"
							element={
								// <OrdersDataContextProvider>
								<Products /> // sem context em produtos
								// </OrdersDataContextProvider>
							}
						/>
						<Route path="/editar-produto/:id" element={<EditProduct />} />

						{/* <Route path="/about" element={<About />} /> */}
					</Route>
				</Routes>
			</Router>
		</OrdersDataContextProvider>
	)
}

export default App
