import { Link, Outlet } from "react-router-dom"
import { FaRegQuestionCircle } from "react-icons/fa"

export function Layout() {
	return (
		<>
			<nav className="flex justify-between items-center bg-blue-700 text-white px-2">
				<div className="flex gap-4">
					<Link to="/">Home</Link>
					<Link to="/about" className="flex items-center gap-1">
						Saiba mais
						<FaRegQuestionCircle />
					</Link>
				</div>
				<p className="text-end text-sm">by Paulo Godoi & Onion</p>
			</nav>
			<main>
				<Outlet />
			</main>
		</>
	)
}
