import { Link, Outlet } from "react-router-dom"
import { FaRegQuestionCircle } from "react-icons/fa"

export function Layout() {
	return (
		<>
			<nav className="bg-blue-700 text-white w-screen">
				<div className="flex justify-between items-center mx-auto max-w-4xl">
					<div className="flex gap-4">
						<Link to="/">ChartApp</Link>
						<Link to="/about" className="flex items-center gap-1">
							Saiba mais
							<FaRegQuestionCircle />
						</Link>
					</div>
				</div>
			</nav>
			<main>
				<Outlet />
			</main>
			<footer className="fixed bottom-0 w-screen p-3">
				<div className="text-end text-sm flex gap-2 justify-end">
					<span className="text-blue-700">
						<a href="https://www.hubcount.com.br/" target="_blank">
							HubCount
						</a>
					</span>
					<span>by Paulo Godoi & Onion</span>
				</div>
			</footer>
		</>
	)
}
