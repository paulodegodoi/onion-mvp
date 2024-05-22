import { ChangeEvent, FormEvent } from "react"
import { Button } from "./Button"
import { Link } from "react-router-dom"

interface IProdutoForm {
	onSubmit: (event: FormEvent<HTMLFormElement>) => Promise<void>
	productName: string
	setProductName: (value: string) => void
	productValue: number
	setProductValue: (value: number) => void
	setIsCreating?: (bol: boolean) => void
}

export function ProdutoForm({
	onSubmit,
	productName,
	setProductName,
	productValue,
	setProductValue,
	setIsCreating,
}: IProdutoForm) {
	return (
		<form
			className="p-3 flex flex-col items-center justify-center w-screen sm:w-8/12 md:w-6/12"
			onSubmit={onSubmit}
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
					onChange={(e: ChangeEvent<HTMLInputElement>) => setProductName(e.target.value)}
					required
					onInvalid={(e: any) => e.target.setCustomValidity("Informe o nome do produto")} // any just to remove ide error
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
				{setIsCreating != null ? (
					<Button
						type="button"
						bgColor="gray"
						text="Voltar"
						onClick={() => setIsCreating(false)}
					/>
				) : (
					<Link to="/produtos">
						<Button type="button" bgColor="gray" text="Voltar" />
					</Link>
				)}
				<Button type="submit" bgColor="green" text="Salvar" />
			</div>
		</form>
	)
}
