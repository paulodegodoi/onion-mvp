import { Link, Outlet } from "react-router-dom"
import { FaRegQuestionCircle } from "react-icons/fa"

export function Layout() {
	return (
		<div className="flex flex-col min-h-screen">
			<nav className="bg-blue-700 text-white w-screen sticky top-0 z-50 h-10 px-3">
				<div className="flex justify-between items-center h-full mx-auto max-w-4xl">
					<Link to="/">ChartApp</Link>
					<Link to="/about" className="flex items-center gap-1">
						Saiba mais
						<FaRegQuestionCircle />
					</Link>
				</div>
			</nav>
			<main>
				<Outlet />
			</main>
			<footer className="bottom-0  mt-auto w-screen p-3 bg-gray-300 opacity-90 z-50 h-10">
				<div className="text-end text-sm flex gap-2 justify-end">
					<span className="text-blue-700">
						<a href="https://www.hubcount.com.br/" target="_blank">
							HubCount
						</a>
					</span>
					<span>by Paulo Godoi & Onion</span>
				</div>
			</footer>
		</div>
	)
}
