/** @type {import('tailwindcss').Config} */
export default {
	content: ["./src/**/*.{js,jsx,ts,tsx}"],
	theme: {
		extend: {
			colors: {
				blue: {
					700: "#07447F",
					900: "#022445",
				},
			},
		},
	},
	plugins: [],
	safelist: [
		{
			// This includes bg of all colors and shades and helps bug with dynamic classes https://stackoverflow.com/a/75907974/24826640
			pattern: /bg-+/,
			variants: ["hover"],
		},
	],
}
