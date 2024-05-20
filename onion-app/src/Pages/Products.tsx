import { useEffect, useState } from "react"
import { Produto } from "../types/produto"
import { formatToCurrency } from "../functions/myFunctions"

export function Products() {
	const [products, setProducts] = useState<Produto[] | null>(null)

	useEffect(() => {
		// Define the async function
		const fetchProducts = async () => {
			try {
				const response = await fetch("http://192.168.0.67:5111/api/Produtos/", {
					method: "GET",
				})
				if (response.ok) {
					const json = await response.json()
					setProducts(json)
				} else {
					alert("Não foi possível processar sua requisição.")
				}
			} catch (error) {
				console.error("Falha ao obter os produtos: ", error)
				alert("Falha ao obter os produtos.")
			}
		}

		// Call the async function
		fetchProducts()
	}, [])

	return (
		<>
			<div className="px-2 w-screen text-center mt-4">
				<h1>Nossos produtos</h1>

				{products != null && (
					<table className="table-auto w-full m-auto border-2 border-blue-900 my-4 text-xs md:text-xl md:w-10/12">
						<thead className="bg-blue-700 text-white">
							<tr>
								<th>Nome</th>
								<th>Preço</th>
							</tr>
						</thead>
						<tbody>
							{products.map((p, index) => (
								<tr
									key={p.id}
									className={`${index + 1 < products.length && "border-b-2"}`}
								>
									<td>{p.nome.toUpperCase()}</td>
									<td>{formatToCurrency(p.valor)}</td>
								</tr>
							))}
						</tbody>
					</table>
				)}
			</div>
		</>
	)
}
