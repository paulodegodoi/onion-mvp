import { FormEvent, useEffect, useState } from "react"
import { Produto } from "../types/produto"
import { formatToCurrency } from "../functions/myFunctions"
import { Button } from "../Components/Button"
import { Loading } from "../Components/Loading"
import { Alert } from "../Components/Alert"
import { BsFillGearFill } from "react-icons/bs"
import { ProdutoForm } from "../Components/ProdutoForm"
import { Link } from "react-router-dom"
import { RiPencilFill } from "react-icons/ri"

export function Products() {
	const [products, setProducts] = useState<Produto[]>([])
	const [isCreating, setIsCreating] = useState(false)
	const [isLoadingData, setIsLoadingData] = useState(false)
	const [isShowAlert, setIsShowAlert] = useState(false)
	const [message, setMessage] = useState("")
	const [productName, setProductName] = useState("")
	const [productValue, setProductValue] = useState(0)

	// carregar produtos
	useEffect(() => {
		const fetchProducts = async () => {
			try {
				const response = await fetch("http://localhost:5111/api/produtos/", {
					method: "GET",
				})
				if (response.ok) {
					const json = await response.json()
					setProducts(json)
				} else {
					const errorText = await response.text()
					console.log(errorText)
					throw new Error(errorText)
				}
			} catch (error) {
				// mostra a mensagem de erro
				setIsShowAlert(true)
				setMessage((error as Error).message)
			}
		}

		fetchProducts()
	}, [])

	// criar o produto
	async function handleCreateProduct(event: FormEvent<HTMLFormElement>) {
		event?.preventDefault()

		const produto: Produto = {
			nome: productName,
			valor: productValue,
		}

		setIsLoadingData(true)
		try {
			const response = await fetch("http://localhost:5111/api/produtos/", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(produto),
			})

			if (response.ok) {
				const jsonResponse: Produto = await response.json()
				setProducts((prevProducts) => [...prevProducts, jsonResponse])
				setProductName("")
				setProductValue(0)
				setIsShowAlert(true)
				setMessage("Produto criado com sucesso!")
			} else {
				setIsLoadingData(false)
				let errorText = await response.text()
				if (errorText == "Failed to fetch")
					errorText = "Falha na comunicação com o servidor"
				throw new Error(errorText)
			}
		} catch (error) {
			// mostra a mensagem de erro
			setIsShowAlert(true)
			setMessage((error as Error).message)
		}
		setIsLoadingData(false)
	}
	return (
		<>
			{isShowAlert && <Alert message={message} setIsShowAlert={setIsShowAlert} />}

			{isCreating ? (
				// criar produto
				<>
					{isLoadingData && <Loading message="Salvando produto..." />}

					<div className="mt-3 flex flex-col items-center">
						<h1 className="text-center text-2xl">Adicionar produto</h1>

						<ProdutoForm
							onSubmit={handleCreateProduct}
							productName={productName}
							setProductName={setProductName}
							productValue={productValue}
							setProductValue={setProductValue}
							setIsCreating={setIsCreating}
						/>
					</div>
				</>
			) : (
				// exibir produtos
				<div className="px-2 w-screen text-center mt-4">
					<div className="mb-3">
						<Button
							bgColor="blue"
							type="button"
							text="Novo Produto"
							onClick={() => setIsCreating(true)!}
						/>
					</div>
					<h1 className="text-3xl">Nossos produtos</h1>

					{products != null && (
						<table className="table-auto w-full m-auto border-2 border-blue-900 my-4 text-md md:text-xl md:w-10/12">
							<thead className="bg-blue-700 text-white">
								<tr>
									<th>Nome</th>
									<th>Preço</th>
									<th>Gerenciar</th>
								</tr>
							</thead>
							<tbody>
								{products.map((p, index) => (
									<tr
										key={p.id}
										className={`${index + 1 < products.length && "border-b-2"}`}
									>
										<td>{p.nome}</td>
										<td>{formatToCurrency(p.valor)}</td>
										<td className="flex justify-center items-center gap-2 pt-1">
											<Link to={`/detalhes-produto/${p.id}`}>
												<BsFillGearFill
													className=" text-gray-500 cursor-pointer"
													title="Gerenciar produto"
												/>
											</Link>
											<Link to={`/editar-produto/${p.id}`}>
												<RiPencilFill
													className="text-gray-500 cursor-pointer"
													title="Detalhes"
												/>
											</Link>
										</td>
									</tr>
								))}
							</tbody>
						</table>
					)}
				</div>
			)}
		</>
	)
}
