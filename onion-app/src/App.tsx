import { Route, HashRouter as Router, Routes } from "react-router-dom"
import { Home } from "./Pages/Home"
import { Layout } from "./Layout"
import { About } from "./Pages/About"

function App() {
	return (
		<Router>
			<Routes>
				<Route element={<Layout />}>
					<Route path="/" element={<Home />}></Route>
					<Route path="/about" element={<About />}></Route>
				</Route>
			</Routes>
		</Router>
	)
}

export default App
