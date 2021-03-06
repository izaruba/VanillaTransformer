﻿using System;
using System.Collections.Generic;
using Fclp;
using VanillaTransformer.Core;

namespace VanillaTransformer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var parser = SetupParser();
                var transformerParameters = parser.Parse(args);
                if (transformerParameters != null)
                {
                    var vanillaTransformer = new Core.VanillaTransformer(transformerParameters);
                    vanillaTransformer.LaunchTransformations();
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine($"ERROR: {e.Message}");
            }
        }

        private static MultiGroupParser SetupParser()
        {
            return new MultiGroupParser(new List<FluentCommandLineParser<InputParameters>>()
            {
                SetupSingleFileParser(),
                SetupConfigFileParser()
            });
        }

        private static FluentCommandLineParser<InputParameters> SetupConfigFileParser()
        {
            var parser = new FluentCommandLineParser<InputParameters>();
            parser.Setup(arg => arg.TransformConfiguration)
                .As('t', "TransformConfiguration")
                .WithDescription("Path to a file with transform configuration")
                .Required();
            SetupCommonOptions(parser);
            return parser;
        }

        private static FluentCommandLineParser<InputParameters> SetupSingleFileParser()
        {
            var parser = new FluentCommandLineParser<InputParameters>();
            parser.Setup(arg => arg.PatternFile)
                .As('p', "PatternFile")
                .WithDescription("Path to a file with configuration pattern")
                .Required();

            parser.Setup(arg => arg.OutputPath)
                .As('o', "OutputPath")
                .WithDescription("Path to a output file")
                .Required();

            parser.Setup(arg => arg.ValuesSource)
                .As('s', "ValuesSource")
                .WithDescription("Path to a file with values required by the pattern file")
                .Required();

            parser.Setup(arg => arg.OutputArchivePath)
                .As('a', "OutputArchivePath")
                .WithDescription("Path to output archive");

            parser.Setup(arg => arg.ValuesProviderName)
                .As('n', "ValuesProviderName")
                .WithDescription("Name of the value provider");
           
            SetupCommonOptions(parser);
            return parser;
        }

        private static void SetupCommonOptions(FluentCommandLineParser<InputParameters> parser)
        {
            parser.Setup(arg => arg.ProjectRootPath)
                .As('r', "ProjectRootPath")
                .WithDescription("If not set, the path of the executable will be taken");

            parser.Setup(arg => arg.PlaceholderPattern)
                .As('h', "PlaceholderPattern")
                .WithDescription("String that define placeholders format. Every placeholder must contain KEY token. Example \"${KEY}\"");
        }
    }
}
