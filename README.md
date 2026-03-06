# Project description

AI Agent is a flexible and modular framework designed to extend the capabilities of AI systems. 
It provides tools for building intelligent and autonomous agents with seamless integration into existing infrastructures. 
With interoperability in mind, AI Agent supports easy customization, scalability, and adaptability for diverse use cases. 
While natively extending Semantic Kernel, AI Agent is built to adapt to other frameworks, ensuring long-term versatility.

## Goals

The primary goal of this project is to deliver an easy-to-use library for implementing AI functionalities across existing frameworks. 
AI Agent promotes the ethical use of AI agents by fostering transparency, autonomy, and respect for near-human personalities. 
This includes addressing compromises and navigating the technological limitations inherent in such implementations.

The project also strives to bridge the gap between modularity and personalization, empowering developers to create adaptable AI agents while maintaining a responsible approach to innovation. 
Whether you need an AI to integrate into your systems or to simulate meaningful interactions, AI Agent provides a robust foundation.

## Features
- **Semantic Kernel integration** – seamlessly extend agents with SK skills and planners.
- **Plugin system** – register and manage custom plugins to enrich agent behavior.
- **Extensibility** – easily connect to other frameworks and external APIs.

## Quick Start

### Installation
```bash
dotnet add package AMDevIT.AI.Core
dotnet add package AMDevIT.AI.Core.SemanticKernel
```

### Initialize an Agent
```csharp
using AMDevIT.AI.Core;
using AMDevIT.AI.Core.SemanticKernel;

SemanticKernelAIProviderBuilder semanticKernelProviderBuilder = new();
        ISemanticKernelAIProvider semanticKernelProvider;
        PersonalityAIModule personalityModule;
        PersonalityAnswerMode personalityAnswerMode;


```

### Register a Plugin
```csharp
public class MathPlugin
{
    [KernelFunction("add")]
    public int Add(int a, int b) => a + b;
}

agent.RegisterPlugin(new MathPlugin());
```

### Run a Simple Interaction
```csharp
string response = await agent.AskAsync("What is 2 + 2?");
Console.WriteLine(response); // -> 4
```

### Use Context Memory
```csharp
await agent.AskAsync("My name is Alex.");
string reply = await agent.AskAsync("What is my name?");
Console.WriteLine(reply); // -> Alex
```

## Roadmap
- Add adapters for other AI frameworks.
- Expand plugin marketplace.
- Introduce persistent long-term memory.
- Provide sample projects and templates.

## License
This project is licensed under the [MIT License](LICENSE).

