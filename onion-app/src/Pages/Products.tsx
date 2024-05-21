import { ChangeEvent, FormEvent, useEffect, useState } from "react"
import { Produto } from "../types/produto"
import { formatToCurrency } from "../functions/myFunctions"
import { Button } from "../Components/Button"
import { Loading } from "../Components/Loading"
import { Alert } from "../Components/Alert"
import { BsFillGearFill } from "react-icons/bs"

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
				const response = await fetch("http://192.168.0.67:5111/api/produtos/", {
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
				// alert(`Ocorreu um erro: ${errorText}`)
			}
		}

		fetchProducts()
	}, [])

	// criar o produto
	async function handleCreateProduct(event: FormEvent<HTMLFormElement>) {
		event?.preventDefault()
		// if (!productName || productName == "") {
		// 	alert("Informe o nome do produto.")
		// 	return
		// }

		const produto: Produto = {
			nome: productName,
			valor: productValue,
		}

		setIsLoadingData(true)
		try {
			const response = await fetch("http://192.168.0.67:5111/api/produtos/", {
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
				// setIsHasError(true)
				// // mostra a mensagem de erro
				// const errorText = await response.text()
				// setMessage(errorText)
				// alert(`Ocorreu um erro: ${errorText}`)
			}
		} catch (error) {
			// console.error("Falha ao adicionar o produto: ", error)
			// alert("Falha ao adicionar o produto")
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

						<form
							className="p-3 flex flex-col items-center justify-center w-screen sm:w-6/12"
							onSubmit={handleCreateProduct}
						>
							<div className="flex flex-col w-full">
								<label className="text-xl" htmlFor="nome">
									Nome
								</label>
								<input
									className=" border border-blue-900 rounded focus:border-2 focus:bg-blue-100 transition-all w-full mb-3 pl-3 focus:py-2 text-xl"
									type="text"
									name="nome"
									id="nome"
									value={productName}
									onChange={(e: ChangeEvent<HTMLInputElement>) =>
										setProductName(e.target.value)
									}
									required
									onInvalid={(e: any) =>
										e.target.setCustomValidity("Informe o nome do produto")
									} // any just to remove ide error
									onInput={(e: any) => e.target.setCustomValidity("")}
								/>
								<label className="text-xl" htmlFor="preco">
									Preço
								</label>
								<input
									className="border border-blue-900 rounded focus:border-2 focus:bg-blue-100 transition-all w-full mb-3 pl-3 focus:py-2 text-xl"
									type="number"
									name="preco"
									id="preco"
									min="1"
									max="99999"
									value={productValue}
									onChange={(e: ChangeEvent<HTMLInputElement>) =>
										setProductValue(Number(e.target.value))
									}
									required
									onInvalid={(e: any) =>
										e.target.setCustomValidity(
											"Informe um valor em reais válido para o produto"
										)
									} // any just to remove ide error
									onInput={(e: any) => e.target.setCustomValidity("")}
									step="0.01"
								/>
							</div>
							<div className="flex justify-between w-full">
								<Button
									type="button"
									bgColor="gray"
									text="Voltar"
									onClick={() => setIsCreating(false)}
								/>
								<Button type="submit" bgColor="green" text="Salvar" />
							</div>
						</form>
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
										<td>{p.nome.toUpperCase()}</td>
										<td>{formatToCurrency(p.valor)}</td>
										<td className="flex justify-center items-center pt-1">
											<BsFillGearFill className="cursor-pointer" />
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
