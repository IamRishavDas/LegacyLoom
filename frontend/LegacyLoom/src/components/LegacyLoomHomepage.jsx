import React, { useEffect, useRef, useState } from 'react';
import { BookOpen, Heart, Users, Shield, ArrowRight, Sparkles, Clock, Star } from 'lucide-react';

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
      icon: <BookOpen className="w-8 h-8" />,
      title: "Intuitive Storytelling",
      description: "Craft your narrative with our effortless, canvas-like interface designed for natural expression."
    },
    {
      icon: <Shield className="w-8 h-8" />,
      title: "Secure & Private",
      description: "Your cherished memories remain yours alone with enterprise-grade security and privacy."
    },
    {
      icon: <Users className="w-8 h-8" />,
      title: "Connect & Share",
      description: "Find others who share your journey and build meaningful connections through stories."
    },
    {
      icon: <Clock className="w-8 h-8" />,
      title: "Eternal Preservation",
      description: "Your legacy endures for generations with our advanced preservation technology."
    }
  ];

  const testimonials = [
    {
      name: "Sarah Mitchell",
      role: "Family Historian",
      content: "Legacy Loom helped me preserve three generations of family stories. It's like having a digital family tree that breathes with life.",
      rating: 5
    },
    {
      name: "Marcus Chen",
      role: "Author",
      content: "The intuitive interface makes storytelling feel natural. I've never found a platform that understands narrative flow like this.",
      rating: 5
    },
    {
      name: "Elena Rodriguez",
      role: "Grandmother",
      content: "Now my grandchildren can hear my voice telling our family's story, even when I'm not there. Priceless.",
      rating: 5
    }
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50">
      {/* Navigation */}
      <nav className="fixed top-0 w-full bg-white/80 backdrop-blur-lg border-b border-gray-200/20 z-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            <div className="flex items-center space-x-2">
              <div className="w-10 h-10 bg-gradient-to-br from-indigo-600 to-purple-600 rounded-lg flex items-center justify-center">
                <Sparkles className="w-6 h-6 text-white" />
              </div>
              <span className="text-2xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
                Legacy Loom
              </span>
            </div>
            
            <div className="hidden md:flex items-center space-x-8">
              <a href="#features" className="text-gray-600 hover:text-indigo-600 transition-colors duration-200">Features</a>
              <a href="#testimonials" className="text-gray-600 hover:text-indigo-600 transition-colors duration-200">Stories</a>
            </div>

            <div className="flex items-center space-x-4">
              <button className="text-gray-600 hover:text-indigo-600 font-medium transition-colors duration-200">
                Login
              </button>
              <button className="bg-gradient-to-r from-indigo-600 to-purple-600 text-white px-6 py-2 rounded-full hover:from-indigo-700 hover:to-purple-700 transition-all duration-200 transform hover:scale-105 shadow-lg hover:shadow-xl">
                Get Started
              </button>
            </div>
          </div>
        </div>
      </nav>

      {/* Hero Section */}
      <section ref={heroRef} className="pt-24 pb-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="text-center">
            <div className={`transition-all duration-1000 transform ${isVisible ? 'translate-y-0 opacity-100' : 'translate-y-10 opacity-0'}`}>
              <h1 className="text-5xl md:text-7xl font-bold text-gray-900 mb-6 leading-tight">
                Your Stories,
                <span className="bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent block">
                  Forever Woven
                </span>
              </h1>
              <p className="text-xl md:text-2xl text-gray-600 mb-12 max-w-4xl mx-auto leading-relaxed">
                Legacy Loom is more than an application—it's a sanctuary for stories, where every memory, dream, and aspiration finds a home that endures for generations.
              </p>
            </div>

            <div className={`transition-all duration-1000 delay-300 transform ${isVisible ? 'translate-y-0 opacity-100' : 'translate-y-10 opacity-0'}`}>
              <div className="flex flex-col sm:flex-row gap-4 justify-center items-center mb-16">
                <button className="bg-gradient-to-r from-indigo-600 to-purple-600 text-white px-8 py-4 rounded-full text-lg font-semibold hover:from-indigo-700 hover:to-purple-700 transition-all duration-200 transform hover:scale-105 shadow-lg hover:shadow-xl flex items-center space-x-2">
                  <span>Start Your Legacy</span>
                  <ArrowRight className="w-5 h-5" />
                </button>
              </div>
            </div>

            {/* Floating Elements */}
            <div className="relative">
              <div className={`transition-all duration-1500 delay-500 transform ${isVisible ? 'translate-y-0 opacity-100' : 'translate-y-20 opacity-0'}`}>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-4xl mx-auto">
                  <div className="bg-white/60 backdrop-blur-sm rounded-2xl p-6 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-2">
                    <Heart className="w-12 h-12 text-rose-500 mb-4 mx-auto" />
                    <h3 className="text-lg font-semibold text-gray-800 mb-2">Cherished Memories</h3>
                    <p className="text-gray-600">Preserve life's precious moments in beautiful, interactive stories</p>
                  </div>
                  <div className="bg-white/60 backdrop-blur-sm rounded-2xl p-6 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-2">
                    <Users className="w-12 h-12 text-blue-500 mb-4 mx-auto" />
                    <h3 className="text-lg font-semibold text-gray-800 mb-2">Family Connections</h3>
                    <p className="text-gray-600">Connect generations through shared stories and experiences</p>
                  </div>
                  <div className="bg-white/60 backdrop-blur-sm rounded-2xl p-6 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-2">
                    <Sparkles className="w-12 h-12 text-purple-500 mb-4 mx-auto" />
                    <h3 className="text-lg font-semibold text-gray-800 mb-2">Lasting Legacy</h3>
                    <p className="text-gray-600">Create something beautiful that will endure for generations</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section id="features" ref={featuresRef} className="py-20 bg-white/50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold text-gray-900 mb-6">
              Crafted for Your Stories
            </h2>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto">
              Every feature is designed with intention, creating a seamless experience that honors your memories while making storytelling effortless.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            {features.map((feature, index) => (
              <div
                key={index}
                className="bg-white rounded-2xl p-8 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-2 group"
              >
                <div className="text-indigo-600 mb-6 group-hover:scale-110 transition-transform duration-300">
                  {feature.icon}
                </div>
                <h3 className="text-xl font-semibold text-gray-800 mb-4">{feature.title}</h3>
                <p className="text-gray-600 leading-relaxed">{feature.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Testimonials Section */}
      <section id="testimonials" className="py-20 bg-gradient-to-r from-indigo-50 to-purple-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold text-gray-900 mb-6">
              Stories That Touch Hearts
            </h2>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto">
              Discover how Legacy Loom has helped families and individuals preserve their most precious memories.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {testimonials.map((testimonial, index) => (
              <div
                key={index}
                className="bg-white rounded-2xl p-8 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-2"
              >
                <div className="flex items-center space-x-1 mb-6">
                  {[...Array(testimonial.rating)].map((_, i) => (
                    <Star key={i} className="w-5 h-5 text-yellow-400 fill-current" />
                  ))}
                </div>
                <p className="text-gray-700 mb-6 leading-relaxed italic">"{testimonial.content}"</p>
                <div>
                  <h4 className="font-semibold text-gray-800">{testimonial.name}</h4>
                  <p className="text-gray-600 text-sm">{testimonial.role}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-20 bg-gradient-to-r from-indigo-600 to-purple-600">
        <div className="max-w-4xl mx-auto text-center px-4 sm:px-6 lg:px-8">
          <h2 className="text-4xl md:text-5xl font-bold text-white mb-6">
            Begin Your Legacy Today
          </h2>
          <p className="text-xl text-indigo-100 mb-12 leading-relaxed">
            Join thousands who have already started preserving their stories. Your legacy awaits, beautifully crafted and eternally accessible.
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center items-center">
            <button className="bg-white text-indigo-600 px-8 py-4 rounded-full text-lg font-semibold hover:bg-gray-50 transition-all duration-200 transform hover:scale-105 shadow-lg hover:shadow-xl flex items-center space-x-2">
              <span>Register Now</span>
              <ArrowRight className="w-5 h-5" />
            </button>
            <button className="border-2 border-white text-white px-8 py-4 rounded-full text-lg font-semibold hover:bg-white hover:text-indigo-600 transition-all duration-200 transform hover:scale-105">
              Login to Continue
            </button>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-gray-900 text-white py-16">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            <div className="col-span-1 md:col-span-2">
              <div className="flex items-center space-x-2 mb-6">
                <div className="w-10 h-10 bg-gradient-to-br from-indigo-600 to-purple-600 rounded-lg flex items-center justify-center">
                  <Sparkles className="w-6 h-6 text-white" />
                </div>
                <span className="text-2xl font-bold">Legacy Loom</span>
              </div>
              <p className="text-gray-400 leading-relaxed max-w-md">
                A sanctuary for stories, where every memory finds a home and every legacy endures for generations to come.
              </p>
            </div>
          </div>
          <div className="border-t border-gray-800 mt-12 pt-8 text-center text-gray-400">
            <p>&copy; 2025 Legacy Loom. All rights reserved. Your stories, forever woven.</p>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default LegacyLoomHomepage;