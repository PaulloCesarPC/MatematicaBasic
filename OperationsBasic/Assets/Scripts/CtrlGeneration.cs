using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CtrlGeneration : MonoBehaviour {
    enum operacoes { adi, sub, mult, div };
    operacoes operacao;



    [SerializeField] GameObject canvas;
    [SerializeField] GameObject Pbox;
    [SerializeField] GameObject Ptext;
    [SerializeField] GameObject[] positions;

    int[] nums = new int[5];
    public GameObject barra;
    float tempTime = 0;

    bool fimJogo = false;
    string texto;
    public GameObject[] posBlocks = new GameObject[5];
    public GameObject[] posBlocks2 = new GameObject[5];
    int sup2 = 0;
    int[] Seqnums = new int[7];
    public GameObject TelaGameOver;
    public Text MsgFinal;
    public Text TempoFinal;
    public Text Tempo;

    public bool gameOver = false;
    bool proporcional = false;

    void Start() {
        RandomNumDecimal();
        CriarBlocks();
    }
    void RandomNumDecimal() {
        //operacao = (operacoes)Random.Range(0, PlayerPrefs.GetInt("op"));
        operacao = (operacoes)PlayerPrefs.GetInt("op");
        nums[1] = 3;//grava a id da operacao

        if (nums[1] == 3)//Divisao
        {
            int resto = 100000;

            do
            {
                nums[0] = Random.Range(1,10);
                nums[2] = Random.Range(1, 10);
                resto = nums[0] % nums[2];

            } while (!(resto == 0 && nums[0] != nums[2]));

            //print("nums[0]: " + nums[0] + "nums[2]: " + nums[2]);
        }
        else
        {
            nums[0] = Random.Range(1, 10);
            nums[2] = Random.Range(1, 10);
        }

        nums[3] = 0;//Igual
        nums[4] = realizarCalc(nums[0], nums[2]);//Salva o resultado da operacao dos nums sorteados

        //for (int i = 0; i < 5; i++)
        //{
        //    if (i == 0 || i == 2 || i == 4)
        //        print(nums[i]);
        //}
    }
    bool VerificaSeNumExiste(int num)
    {
        for (int i = 0; i < posBlocks2.Length; i++)
        {
            if (Seqnums[i] == num)
                return true;
        }
        return false;
    }

    int NumRandomico()
    {
        int numero;
        do
            numero = Random.Range(0, 5);
        while (VerificaSeNumExiste(numero));
        return numero;
    }

    //Num1,sinal,Num2,igual,resultado.
    void CriarBlocks() {
        for (int i = 0; i < posBlocks2.Length; i++)
            Seqnums[i] = -1;

        for (int i = 0; i < posBlocks2.Length; i++)
        {
            Seqnums[i] = NumRandomico();
            InstantiateObj(i, Pbox, positions);
        }

        
        if (nums[0] / nums[4] == nums[2] &&  nums[0] / nums[2] == nums[4])
        {
            print("proporcional");
            proporcional = true;
        }
           

    
     

        //for (int i = 0; i < posBlocks.Length; i++)
        //    print("Ordem Correta:" + posBlocks[i].transform.position);  // Ordem Correta

        for (int i = 0; i < posBlocks.Length; i++)
            print("Random: " + Seqnums[i]);

        for (int i = 0; i < posBlocks2.Length; i++)
            print("Ordem do Jogo: " + posBlocks2[i].transform.position); // Ordem do Jogo
    }


    void Update() {
        if (!gameOver)
        {
            tempTime += Time.deltaTime;
            float percentual = (tempTime / 30) * 1;//1 = tamanho barra
            float tamanhoBarra = 1 - percentual;//1 tbm do tamanho
            barra.transform.localScale = new Vector3(tamanhoBarra, -0.08f, 1);

            if ((barra.transform.localScale.x < 0))
            {
                barra.transform.localScale = new Vector3(0, -0.08f, 1);
                LimparTextos();
                Derrota();
            }
            Tempo.text = (30 - ((int)tempTime)).ToString() + "'";
        }
    }

    int realizarCalc(int num1, int num2) {
        int resultado = 0;
        switch (operacao) {
            case operacoes.adi:
                resultado = num1 + num2;
                texto = "+";
                break;
            case operacoes.sub:
                resultado = num1 - num2;
                texto = "-";
                break;
            case operacoes.mult:
                resultado = num1 * num2;
                texto = "x";
                break;
            case operacoes.div:
                resultado = (num1 / num2);
                texto = "/";
                break;
        }
        return resultado;
    }


    //for (int i = 0; i<posBlocks.Length; i++)
    //    print("Ordem Correta:" + posBlocks[i].transform.position); // 

    //for (int i = 0; i<posBlocks.Length; i++)
    //    print("Random: " + Seqnums[i]);

    //for (int i = 0; i<posBlocks.Length; i++)
    //    print("Ordem do Jogo: " + posBlocks2[i].transform.position); // Ordem Correta


    void InstantiateObj(int NumOrSymbol, GameObject Pbox, GameObject[] positions) {
        //Instantiete, posicao, Cor da Box.
        GameObject tempPrefab = Instantiate(Pbox) as GameObject;
        tempPrefab.transform.position = positions[Seqnums[NumOrSymbol]].transform.position;
        tempPrefab.transform.GetComponent<SpriteRenderer>().color = new Color(corRandom(), corRandom(), corRandom());

        posBlocks[NumOrSymbol] = tempPrefab;// Ordem Correta
        posBlocks2[Seqnums[NumOrSymbol]] = tempPrefab;// Ordem no Jogo
        //Armazena a posicao do Objeto Criado

        //Texto,link
        GameObject tempTxt = Instantiate(Ptext) as GameObject;
        tempTxt.transform.SetParent(canvas.transform);
        tempTxt.GetComponent<necklaceText>().obj = tempPrefab;

        switch (NumOrSymbol)
        {
            case 1://Sinal           
                tempTxt.GetComponent<Text>().text = texto;
                break;
            case 3://Igual
                tempTxt.GetComponent<Text>().text = "=";
                break;
            case 0://Normal
            case 2:
            case 4:
                tempTxt.GetComponent<Text>().text = nums[sup2].ToString();
                sup2 += 2;
                //Num1 0,sinal 1 ,Num2  2 ,igual 3 ,resultado 4.     
                break;
        }
        //Contador de Instancias Realizadas
    }
    float corRandom() {
        return UnityEngine.Random.Range(0, 1f);
    }

    public void AtivarVerificador()
    {
        VerificarVitoria();
    }

    void FimJogo()
    {
        gameOver = true;
        Invoke("LimparTextos", 4);
        Invoke("Vitoria", 4);
    }
    void VerificarVitoria() {
        if (nums[1] == 0 || nums[1] == 2) {//Soma ou Multi
            if (((posBlocks2[Seqnums[0]].transform.position.x < posBlocks2[Seqnums[1]].transform.position.x) &&
            (posBlocks2[Seqnums[1]].transform.position.x < posBlocks2[Seqnums[2]].transform.position.x) ||

            (posBlocks2[Seqnums[2]].transform.position.x < posBlocks2[Seqnums[1]].transform.position.x) &&
            (posBlocks2[Seqnums[1]].transform.position.x < posBlocks2[Seqnums[0]].transform.position.x))

            &&

            (posBlocks2[Seqnums[2]].transform.position.x < posBlocks2[Seqnums[3]].transform.position.x) &&
            (posBlocks2[Seqnums[3]].transform.position.x < posBlocks2[Seqnums[4]].transform.position.x))
            {
                Invoke("FimJogo", 1);
            }
        }
        else
        {
            if (nums[1] == 3 && proporcional)
            {
                if (
                    //1 numero                                   //Sinal
                    (posBlocks2[Seqnums[0]].transform.position.x < posBlocks2[Seqnums[1]].transform.position.x) &&
                    (

                        //Sinal                                    //2 numero    
                        (posBlocks2[Seqnums[1]].transform.position.x < posBlocks2[Seqnums[2]].transform.position.x)

                        &&
                        //2 numero                          //Igual
                        (posBlocks2[Seqnums[2]].transform.position.x < posBlocks2[Seqnums[3]].transform.position.x) &&

                        //Igual                                             //Resultado
                        (posBlocks2[Seqnums[3]].transform.position.x < posBlocks2[Seqnums[4]].transform.position.x) 

                     

                    ) ||

                        //Sinal                                         //Resultado
                        (posBlocks2[Seqnums[1]].transform.position.x < posBlocks2[Seqnums[4]].transform.position.x) &&

                   //Resultado                                         //Igual
                   (posBlocks2[Seqnums[4]].transform.position.x < posBlocks2[Seqnums[3]].transform.position.x)
                     &&
                   
                    //Igual                                             //2 numero    
                    (posBlocks2[Seqnums[3]].transform.position.x < posBlocks2[Seqnums[2]].transform.position.x)
                    
                    
                    )
                {
                    Invoke("FimJogo", 1);
                   
                }
            }
            else
            {
               
                    
                    if ((posBlocks2[Seqnums[0]].transform.position.x < posBlocks2[Seqnums[1]].transform.position.x) &&
                (posBlocks2[Seqnums[1]].transform.position.x < posBlocks2[Seqnums[2]].transform.position.x)

                &&

                (posBlocks2[Seqnums[2]].transform.position.x < posBlocks2[Seqnums[3]].transform.position.x) &&
                (posBlocks2[Seqnums[3]].transform.position.x < posBlocks2[Seqnums[4]].transform.position.x))
                    {
                    Invoke("FimJogo", 1);
                }
                
                //else
                //{
                //   if ((posBlocks2[Seqnums[0]].transform.position.x < posBlocks2[Seqnums[1]].transform.position.x) &&
                //      (posBlocks2[Seqnums[1]].transform.position.x < posBlocks2[Seqnums[2]].transform.position.x)

                //      &&

                //      (posBlocks2[Seqnums[2]].transform.position.x < posBlocks2[Seqnums[3]].transform.position.x) &&
                //      (posBlocks2[Seqnums[3]].transform.position.x < posBlocks2[Seqnums[4]].transform.position.x))
                //    {
                //        gameOver = true;
                //        Invoke("LimparTextos", 6);
                //        Invoke("Vitoria", 6);
                //    }

                //}
            }

        }       
    }

    void LimparTextos(){
         GameObject[] objs = GameObject.FindGameObjectsWithTag("texto");
            for (int i = 0; i<objs.Length; i++)
                Destroy(objs[i]);
    }
    void Vitoria()
    {
        gameOver = true;
        Time.timeScale = 0;
        TelaGameOver.SetActive(true);
        MsgFinal.text = "Parabéns";
        TempoFinal.text = (30 - (int)tempTime).ToString();
    }

    void Derrota()
    {
        gameOver = true;
        Time.timeScale = 0;
        TelaGameOver.SetActive(true);
        MsgFinal.text = "Seu tempo acabou";
        TempoFinal.text = (30 -(int)tempTime).ToString();
    }
}
