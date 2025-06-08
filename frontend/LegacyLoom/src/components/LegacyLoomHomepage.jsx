import { useEffect, useRef, useState } from 'react';
import { Heart, Users, Shield, ArrowRight, Star, Feather, Archive, Home, Snowflake } from 'lucide-react';

const LegacyLoomHomepage = () => {
  const heroRef = useRef(null);
  const featuresRef = useRef(null);
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    // Animate hero elements on load
    const timer = setTimeout(() => {
      setIsVisible(true);
    }, 100);

    return () => clearTimeout(timer);
  }, []);

  const features = [
    {
      icon: <Feather className="w-8 h-8" />,
      title: "Intuitive Storytelling",
      description: "Craft your narrative with our effortless, canvas-like interface designed for natural expression and gentle guidance."
    },
    {
      icon: <Shield className="w-8 h-8" />,
      title: "Secure & Private",
      description: "Your cherished memories remain yours alone with whisper-quiet security that protects without intruding."
    },
    {
      icon: <Users className="w-8 h-8" />,
      title: "Connect & Share",
      description: "Find kindred spirits who share your journey and weave meaningful connections through shared stories."
    },
    {
      icon: <Archive className="w-8 h-8" />,
      title: "Eternal Preservation",
      description: "Your legacy rests safely in our digital sanctuary, preserved with the gentleness of museum care."
    }
  ];

  const testimonials = [
    {
      name: "Sarah Mitchell",
      role: "Family Historian",
      content: "Legacy Loom helped me preserve three generations of family stories. It's like having a digital family tree that breathes with quiet life.",
      rating: 5
    },
    {
      name: "Marcus Chen",
      role: "Author",
      content: "The gentle interface makes storytelling feel like writing in a cherished journal. Every word finds its perfect place.",
      rating: 5
    },
    {
      name: "Elena Rodriguez",
      role: "Grandmother",
      content: "Now my grandchildren can hear the whispers of our family's story, wrapped in love and preserved forever.",
      rating: 5
    }
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-stone-50 via-gray-50 to-slate-100">
      {/* Navigation */}
      <nav className="fixed top-0 w-full bg-white/90 backdrop-blur-xl border-b border-stone-200/50 z-50 shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            <div className="flex items-center space-x-3">
              <div className="w-10 h-10 bg-gradient-to-br from-stone-600 to-slate-700 rounded-xl flex items-center justify-center shadow-lg">
                <Snowflake className="w-5 h-5 text-stone-100" />
              </div>
              <span className="text-[19px] font-serif font-bold text-stone-800 tracking-wide">
                Legacy Loom
              </span>
            </div>
            
            <div className="hidden md:flex items-center space-x-8">
              <a href="#features" className="text-stone-600 hover:text-stone-800 transition-all duration-300 font-medium">Features</a>
              <a href="#testimonials" className="text-stone-600 hover:text-stone-800 transition-all duration-300 font-medium">Stories</a>
            </div>

            <div className="flex items-center space-x-4">
              <button className="text-stone-600 hover:text-stone-800 font-medium transition-all duration-300 px-4 py-2 rounded-lg hover:bg-stone-50">
                Login
              </button>
              <button className="bg-gradient-to-r from-stone-700 to-slate-800 text-stone-50 px-6 py-2 rounded-full hover:from-stone-800 hover:to-slate-900 transition-all duration-300 transform hover:scale-105 shadow-lg hover:shadow-xl font-medium">
                Register
              </button>
            </div>
          </div>
        </div>
      </nav>

      {/* Hero Section */}
      <section ref={heroRef} className="pt-28 pb-24 px-4 sm:px-6 lg:px-8 relative overflow-hidden">
        {/* Animated Background Elements */}
        <div className="absolute inset-0 pointer-events-none">
          <div className="absolute top-20 left-10 w-32 h-32 bg-gradient-to-br from-stone-200/30 to-slate-300/20 rounded-full blur-3xl animate-pulse"></div>
          <div className="absolute top-40 right-20 w-48 h-48 bg-gradient-to-br from-stone-300/20 to-slate-200/30 rounded-full blur-3xl animate-pulse delay-1000"></div>
          <div className="absolute bottom-32 left-1/4 w-40 h-40 bg-gradient-to-br from-stone-200/25 to-slate-300/15 rounded-full blur-3xl animate-pulse delay-2000"></div>
        </div>
        
        <div className="max-w-7xl mx-auto relative">
          <div className="text-center">
            <div className={`transition-all duration-1200 ease-out transform ${isVisible ? 'translate-y-0 opacity-100' : 'translate-y-12 opacity-0'}`}>
              {/* Floating decorative elements */}
              <div className="absolute -top-8 left-1/2 transform -translate-x-1/2 animate-bounce delay-3000">
                {/* <div className='flex gap-5'>
                    <Sparkles className="w-6 h-6 text-stone-400/60" />
                    <Sparkles className="w-6 h-6 text-stone-400/60" />
                    <Sparkles className="w-6 h-6 text-stone-400/60" />
                </div> */}
              </div>
              <h1 className="text-5xl md:text-7xl font-serif font-bold text-stone-800 mb-8 leading-tight relative">
                Your Stories,
                <span className="block text-stone-600 mt-2 bg-gradient-to-r from-stone-600 via-stone-700 to-slate-600 bg-clip-text animate-pulse">
                  Gently Woven
                </span>
                {/* Decorative underline */}
                <div className="absolute -bottom-4 left-1/2 transform -translate-x-1/2 w-32 h-1 bg-gradient-to-r from-transparent via-stone-300 to-transparent rounded-full"></div>
              </h1>
              <p className="text-xl md:text-2xl text-stone-600 mb-16 max-w-4xl mx-auto leading-relaxed font-light">
                Legacy Loom is more than an application—it's a quiet sanctuary for stories, where every whispered memory, 
                gentle dream, and heartfelt aspiration finds a home that endures with grace.
              </p>
            </div>

            {/* Floating Elements */}
            <div className="relative">
              <div className={`transition-all duration-1500 delay-600 ease-out transform ${isVisible ? 'translate-y-0 opacity-100' : 'translate-y-16 opacity-0'}`}>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-5xl mx-auto">
                  <div className="bg-white/70 backdrop-blur-sm rounded-3xl p-8 shadow-lg hover:shadow-xl transition-all duration-500 transform hover:-translate-y-3 border border-stone-200/50">
                    <Heart className="w-14 h-14 text-stone-500 mb-6 mx-auto opacity-80" />
                    <h3 className="text-xl font-serif font-semibold text-stone-800 mb-3">Cherished Memories</h3>
                    <p className="text-stone-600 leading-relaxed">Preserve life's precious moments in beautiful, gentle stories that honor every detail</p>
                  </div>
                  <div className="bg-white/70 backdrop-blur-sm rounded-3xl p-8 shadow-lg hover:shadow-xl transition-all duration-500 transform hover:-translate-y-3 border border-stone-200/50">
                    <Users className="w-14 h-14 text-stone-500 mb-6 mx-auto opacity-80" />
                    <h3 className="text-xl font-serif font-semibold text-stone-800 mb-3">Family Bonds</h3>
                    <p className="text-stone-600 leading-relaxed">Weave generations together through shared experiences and quiet understanding</p>
                  </div>
                  <div className="bg-white/70 backdrop-blur-sm rounded-3xl p-8 shadow-lg hover:shadow-xl transition-all duration-500 transform hover:-translate-y-3 border border-stone-200/50">
                    <Archive className="w-14 h-14 text-stone-500 mb-6 mx-auto opacity-80" />
                    <h3 className="text-xl font-serif font-semibold text-stone-800 mb-3">Lasting Legacy</h3>
                    <p className="text-stone-600 leading-relaxed">Create something beautiful that will rest peacefully through generations</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section id="features" ref={featuresRef} className="py-24 bg-white/60 backdrop-blur-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-20">
            <h2 className="text-4xl md:text-5xl font-serif font-bold text-stone-800 mb-8">
              Crafted with Quiet Care
            </h2>
            <p className="text-xl text-stone-600 max-w-3xl mx-auto leading-relaxed font-light">
              Every feature whispers thoughtfulness, creating a seamless experience that honors your memories 
              while making storytelling feel like a gentle conversation with an old friend.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            {features.map((feature, index) => (
              <div
                key={index}
                className="bg-white/80 rounded-3xl p-10 shadow-lg hover:shadow-xl transition-all duration-500 transform hover:-translate-y-2 group border border-stone-200/30"
              >
                <div className="text-stone-600 mb-8 group-hover:scale-110 transition-transform duration-500 opacity-80">
                  {feature.icon}
                </div>
                <h3 className="text-xl font-serif font-semibold text-stone-800 mb-5">{feature.title}</h3>
                <p className="text-stone-600 leading-relaxed font-light">{feature.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Testimonials Section */}
      <section id="testimonials" className="py-24 bg-gradient-to-br from-stone-100 to-slate-100">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-20">
            <h2 className="text-4xl md:text-5xl font-serif font-bold text-stone-800 mb-8">
              Whispered Testimonials
            </h2>
            <p className="text-xl text-stone-600 max-w-3xl mx-auto leading-relaxed font-light">
              Gentle voices sharing how Legacy Loom has become a cherished companion in preserving 
              their most precious memories with quiet dignity.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-10">
            {testimonials.map((testimonial, index) => (
              <div
                key={index}
                className="bg-white/80 rounded-3xl p-10 shadow-lg hover:shadow-xl transition-all duration-500 transform hover:-translate-y-2 border border-stone-200/30"
              >
                <div className="flex items-center space-x-1 mb-8">
                  {[...Array(testimonial.rating)].map((_, i) => (
                    <Star key={i} className="w-5 h-5 text-amber-400 fill-current opacity-80" />
                  ))}
                </div>
                <p className="text-stone-700 mb-8 leading-relaxed italic font-light text-lg">"{testimonial.content}"</p>
                <div>
                  <h4 className="font-serif font-semibold text-stone-800 text-lg">{testimonial.name}</h4>
                  <p className="text-stone-500 text-sm font-light mt-1">{testimonial.role}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-24 bg-gradient-to-r from-stone-700 to-slate-800">
        <div className="max-w-4xl mx-auto text-center px-4 sm:px-6 lg:px-8">
          <h2 className="text-4xl md:text-5xl font-serif font-bold text-stone-100 mb-8">
            Begin Your Gentle Legacy
          </h2>
          <p className="text-xl text-stone-300 mb-16 leading-relaxed font-light max-w-3xl mx-auto">
            Join the quiet community of storytellers who have found their sanctuary. 
            Your legacy awaits, beautifully crafted and eternally accessible, wrapped in gentle care.
          </p>
          <div className="flex flex-col sm:flex-row gap-6 justify-center items-center">
            <button className="bg-white text-stone-700 px-10 py-4 rounded-full text-lg font-medium hover:bg-stone-50 transition-all duration-300 transform hover:scale-105 shadow-lg hover:shadow-xl flex items-center space-x-3">
              <span>Create Account</span>
              <ArrowRight className="w-5 h-5" />
            </button>
            <button className="border-2 border-stone-300 text-stone-100 px-10 py-4 rounded-full text-lg font-medium hover:bg-white hover:text-stone-700 transition-all duration-300 transform hover:scale-105">
              Sign In
            </button>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-stone-900 text-stone-300 py-20">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-12">
            <div className="col-span-1 md:col-span-2">
              <div className="flex items-center space-x-3 mb-8">
                <div className="w-12 h-12 bg-gradient-to-br from-stone-600 to-slate-700 rounded-xl flex items-center justify-center shadow-lg">
                  <Home className="w-6 h-6 text-stone-100" />
                </div>
                <span className="text-2xl font-serif font-bold text-stone-100">Legacy Loom</span>
              </div>
              <p className="text-stone-400 leading-relaxed max-w-md font-light text-lg">
                A quiet sanctuary for stories, where every memory finds a gentle home 
                and every legacy rests peacefully through the ages.
              </p>
            </div>
          </div>
          <div className="border-t border-stone-800 mt-16 pt-10 text-center text-stone-500">
            <p className="font-light">&copy; 2025 Legacy Loom. All rights reserved. Your stories, gently woven.</p>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default LegacyLoomHomepage;