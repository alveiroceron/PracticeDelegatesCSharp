using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MyApp
{
    class Result
    {
        public string Name { get; set; }
        public int Lenght { get; set; }

        public override string ToString() => $"Name {Name} and lenght {Lenght}";
    }

    class Program
    {
        static void Main(string[] args)
        {
            var names = new string[] { "Alveiro", "Fernando", "Elian", "Felipe", "Mario", "Carlos" };
            var processor = new ProcessorNameCounter(names);

            processor.OnprogressEvent += Processor_OnprogressEvent;
            processor.OnCompleteEvent += (sender, results) => Console.WriteLine($"We processed {results.Count()} elements");



            processor.Execute();
            Console.ReadLine();
        }

        private static void Processor_OnprogressEvent(object sender, float args, Result item)
        {
            Console.Clear();
            Console.WriteLine($"Processing: {item}");
            for (int i = 0; i < args; i++)
            {
                Console.Write("* ");
            }
        }
    }

    class ProcessorNameCounter : IProcessor
    {
        public delegate void OnEmpty(object sender);
        //public delegate void OnComplete(object sender, IEnumerable<Result> args);
        public delegate void OnProgress(object sender, float args, Result item);
        public event OnEmpty OnDataEmptyEvent;    
        //public event OnComplete OnCompleteEvent;
        public event OnProgress OnprogressEvent;

        public Action<object, IEnumerable<Result>> OnCompleteEvent;


        public ProcessorNameCounter(ICollection<string> data)
        {
            Data = data;
        }

        public ICollection<string> Data { get; set; }

        public void Execute()
        {
            if (Data == null || !Data.Any())
            {
                OnDataEmptyEvent?.Invoke(this);
                return;
            }
            var results = new List<Result>();
            var i = 0;
            foreach (var item in Data)
            { 
                Thread.Sleep(2000);
                var result = new Result { Name = item, Lenght = item.Length };
                OnprogressEvent?.Invoke(this, ++i, result);
                results.Add(result);
            }  
            OnCompleteEvent?.Invoke(this, results);
        }

    }

    interface IProcessor
    {
        void Execute();
    }

}