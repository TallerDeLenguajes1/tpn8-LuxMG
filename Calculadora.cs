using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class Calculadora {
	static private float numero1;
	static private float numero2;
	static private string fecha;
	static private string operacion;

	static public float Numero1 { get => numero1; set => numero1 = value; }
	static public float Numero2 { get => numero2; set => numero2 = value; }
	static public string Operacion { get => operacion; set => operacion = value; }
	static public string Fecha { get => fecha; set => fecha = value; }

	public Calculadora (float num1, float num2, string op) {
		numero1 = num1;
		numero2 = num2;
		operacion = op;
		fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
	}

	static public float Suma () {
		return numero1 + numero2;
	}

	static public float Resta () {
		return numero1 - numero2;
	}

	static public float Multiplicacion () {
		return numero1 * numero2;
	}

	static public float Division () {
		if (numero2 != 0) return numero1 / numero2;
		return 0;
	}
}

class MainClass {
	static public LinkedList<string> historial = new LinkedList<string>();

	static public void Main () {
		int continuar = 1;

		do{
			Solucion();
			MostrarHistorial();
			Console.WriteLine("\nDesea realizar otra operacion? (0:no, otra tecla:si)");
			if(Console.ReadLine() == "0") continuar = 0;
			Console.WriteLine("\n");
		}while(continuar!=0);
	}

	static private void Solucion () {
		string pattern = @"(\d+)([.,])?(\d*)([-+*/])(\d+)([.,])?(\d*)";
		string operacion = String.Empty;

		Console.WriteLine("Ingrese la operacion que desea realizar: ");
		operacion = Console.ReadLine();
		operacion = operacion.Replace(" ", "").Replace("=", ""); //para eliminar posibles espacios en blanco
		Match oper = System.Text.RegularExpressions.Regex.Match(operacion, pattern);

		if(oper.Success){
			string val1 = oper.Groups[1].Value + oper.Groups[2].Value + oper.Groups[3].Value;
			float value1 = Convert.ToSingle(val1);
			string val2 = oper.Groups[5].Value + oper.Groups[6].Value + oper.Groups[7].Value;
			float value2 = Convert.ToSingle(val2);
			string operacionN = oper.ToString();
			
			new Calculadora(value1, value2, operacionN);

			float resultado = 0;

			switch (oper.Groups[4].Value){
				case "+":
					resultado = Calculadora.Suma();
					break;
				case "-":
					resultado = Calculadora.Resta();
					break;
				case "*":
					resultado = Calculadora.Multiplicacion();
					break;
				case "/":
					if(Calculadora.Numero2 != 0){
						resultado = Calculadora.Division();
					}else{
						Console.WriteLine("ERROR");
					}
					break;
			}

			if(!(oper.Groups[4].Value == "/" && Calculadora.Numero2 == 0)) Console.WriteLine(operacionN + " = " + resultado);
			
			try{
				string hist;
				if(!(oper.Groups[4].Value == "/" && Calculadora.Numero2 == 0)){
					hist = Calculadora.Fecha + @" --> " + Calculadora.Operacion + " = " + resultado;
				}else{
					hist = Calculadora.Fecha + @" --> " + Calculadora.Operacion + " = ERROR";
				}
				historial.AddFirst(hist);
			}
			catch(NullReferenceException){
				Console.WriteLine("Problemas al agregar al historial");
			}
		}else{
			Console.WriteLine("No se pudo realizar la operacion");
		}	
	}

	static private void MostrarHistorial () {
		Console.WriteLine("\nHistorial:");
		foreach(string s in historial){
			Console.WriteLine(s);
		}
	}
}