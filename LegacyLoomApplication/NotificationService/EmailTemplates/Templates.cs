using System.ComponentModel.DataAnnotations;

namespace NotificationService.EmailTemplates
{
    public enum TemplateName
    {
        WELCOME
    }
    public class Templates
    {
        public string GetTemplate([Required]TemplateName templateName, string? userName)
        {
            List<string> templateContents = new()
            {
                #region WELCOME Email
                $@"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Welcome to Legacy Loom</title>
                    <style>
                        @media only screen and (max-width: 600px) {{
                            .email-container {{
                                width: 100% !important;
                                margin: 0 !important;
                            }}
                            .content-padding {{
                                padding: 30px 20px !important;
                            }}
                            .header-padding {{
                                padding: 30px 20px 20px !important;
                            }}
                            .logo-container {{
                                padding: 15px 20px !important;
                            }}
                            .logo-symbol {{
                                width: 60px !important;
                                height: 45px !important;
                            }}
                            .brand-name {{
                                font-size: 24px !important;
                            }}
                            .tagline {{
                                font-size: 10px !important;
                            }}
                            .hero-text-overlay {{
                                left: 20px !important;
                                right: 20px !important;
                            }}
                            .hero-title {{
                                font-size: 16px !important;
                            }}
                            .hero-subtitle {{
                                font-size: 12px !important;
                            }}
                            .main-title {{
                                font-size: 26px !important;
                                margin-bottom: 20px !important;
                            }}
                            .welcome-box {{
                                padding: 20px !important;
                                margin-bottom: 20px !important;
                            }}
                            .welcome-text {{
                                font-size: 16px !important;
                            }}
                            .welcome-subtext {{
                                font-size: 14px !important;
                            }}
                            .feature-item {{
                                flex-direction: column !important;
                                text-align: center !important;
                                padding: 15px !important;
                            }}
                            .feature-icon {{
                                margin-right: 0 !important;
                                margin-bottom: 15px !important;
                            }}
                            .cta-button {{
                                padding: 15px 30px !important;
                                font-size: 16px !important;
                            }}
                            .support-section {{
                                padding: 20px !important;
                            }}
                            .footer-padding {{
                                padding: 30px 20px !important;
                            }}
                        }}
        
                        @media only screen and (max-width: 480px) {{
                            .main-title {{
                                font-size: 22px !important;
                            }}
                            .welcome-text {{
                                font-size: 15px !important;
                            }}
                            .welcome-subtext {{
                                font-size: 13px !important;
                            }}
                            .hero-image {{
                                height: 200px !important;
                            }}
                            .feature-item {{
                                padding: 12px !important;
                                margin-bottom: 15px !important;
                            }}
                            .feature-title {{
                                font-size: 14px !important;
                            }}
                            .feature-desc {{
                                font-size: 12px !important;
                            }}
                        }}
                    </style>
                </head>
                <body style=""margin: 0; padding: 0; font-family: 'Georgia', 'Times New Roman', serif; background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%); color: #333333;"">
                    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td align=""center"" style=""padding: 40px 20px;"">
                                <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""0"" border=""0"" class=""email-container"" style=""background-color: #ffffff; border-radius: 16px; box-shadow: 0 20px 40px rgba(0,0,0,0.1); overflow: hidden; max-width: 600px; width: 100%;"">
                    
                                    <!-- Header with Enhanced Logo -->
                                    <tr>
                                        <td class=""header-padding"" style=""padding: 50px 40px 30px; text-align: center; background: linear-gradient(135deg, #2c1810 0%, #8b4513 50%, #d2691e 100%); position: relative;"">
                                            <!-- Decorative Pattern Background -->
                                            <div style=""position: absolute; top: 0; left: 0; right: 0; bottom: 0; opacity: 0.1; background-image: url('data:image/svg+xml,<svg xmlns=\""http://www.w3.org/2000/svg\"" viewBox=\""0 0 100 100\""><defs><pattern id=\""weave\"" patternUnits=\""userSpaceOnUse\"" width=\""20\"" height=\""20\""><rect width=\""20\"" height=\""20\"" fill=\""%23ffffff\""/><rect width=\""10\"" height=\""10\"" fill=\""%23000000\"" opacity=\""0.1\""/><rect x=\""10\"" y=\""10\"" width=\""10\"" height=\""10\"" fill=\""%23000000\"" opacity=\""0.1\""/></pattern></defs><rect width=\""100\"" height=\""100\"" fill=\""url(%23weave)\""/></svg>'); background-size: 40px 40px;""></div>
                            
                                            <!-- New Logo Design -->
                                            <div style=""position: relative; z-index: 2;"">
                                                <div class=""logo-container"" style=""display: inline-block; padding: 20px 30px; background: rgba(255,255,255,0.95); border-radius: 12px; box-shadow: 0 8px 32px rgba(0,0,0,0.2); backdrop-filter: blur(10px);"">
                                                    <!-- Logo Symbol: Stylized Loom -->
                                                    <div class=""logo-symbol"" style=""width: 80px; height: 60px; margin: 0 auto 15px; position: relative;"">
                                                        <!-- Loom Frame -->
                                                        <div style=""position: absolute; top: 0; left: 10px; right: 10px; height: 8px; background: linear-gradient(90deg, #8b4513, #d2691e); border-radius: 4px;""></div>
                                                        <div style=""position: absolute; bottom: 0; left: 10px; right: 10px; height: 8px; background: linear-gradient(90deg, #8b4513, #d2691e); border-radius: 4px;""></div>
                                                        <div style=""position: absolute; top: 8px; bottom: 8px; left: 0; width: 8px; background: linear-gradient(180deg, #8b4513, #d2691e); border-radius: 4px;""></div>
                                                        <div style=""position: absolute; top: 8px; bottom: 8px; right: 0; width: 8px; background: linear-gradient(180deg, #8b4513, #d2691e); border-radius: 4px;""></div>
                                        
                                                        <!-- Woven Threads -->
                                                        <div style=""position: absolute; top: 16px; left: 16px; right: 16px; bottom: 16px;"">
                                                            <div style=""position: absolute; top: 0; width: 100%; height: 2px; background: linear-gradient(90deg, #ffd700, #ffed4e); border-radius: 1px;""></div>
                                                            <div style=""position: absolute; top: 8px; width: 100%; height: 2px; background: linear-gradient(90deg, #ff6b6b, #ff8e8e); border-radius: 1px;""></div>
                                                            <div style=""position: absolute; top: 16px; width: 100%; height: 2px; background: linear-gradient(90deg, #4ecdc4, #7fdbda); border-radius: 1px;""></div>
                                                            <div style=""position: absolute; top: 24px; width: 100%; height: 2px; background: linear-gradient(90deg, #45b7d1, #74c0fc); border-radius: 1px;""></div>
                                                        </div>
                                                    </div>
                                    
                                                    <!-- Brand Name -->
                                                    <div class=""brand-name"" style=""font-family: 'Georgia', serif; font-size: 28px; font-weight: bold; color: #2c1810; letter-spacing: 1px; margin-bottom: 5px;"">Legacy Loom</div>
                                                    <div class=""tagline"" style=""font-size: 12px; color: #8b4513; text-transform: uppercase; letter-spacing: 2px; font-weight: 300;"">Weaving Stories • Crafting Memories</div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>

                                    <!-- Hero Image Section -->
                                    <tr>
                                        <td style=""padding: 0; position: relative;"">
                                            <div style=""position: relative; overflow: hidden;"">
                                                <img class=""hero-image"" src=""https://images.unsplash.com/photo-1528569937393-ee892b976859?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"" alt=""Artisan weaving on traditional loom"" style=""width: 100%; height: 300px; object-fit: cover; display: block;"">
                                                <!-- Overlay Gradient -->
                                                <div style=""position: absolute; bottom: 0; left: 0; right: 0; height: 100px; background: linear-gradient(transparent, rgba(0,0,0,0.3));""></div>
                                                <!-- Floating Text -->
                                                <div class=""hero-text-overlay"" style=""position: absolute; bottom: 20px; left: 40px; color: white; text-shadow: 2px 2px 4px rgba(0,0,0,0.5);"">
                                                    <div class=""hero-title"" style=""font-size: 18px; font-weight: bold; margin-bottom: 5px;"">Craftsmanship Meets Digital Innovation</div>
                                                    <div class=""hero-subtitle"" style=""font-size: 14px; opacity: 0.9;"">Where traditions are preserved and stories come alive</div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>

                                    <!-- Main Content -->
                                    <tr>
                                        <td class=""content-padding"" style=""padding: 50px 40px; background: linear-gradient(180deg, #ffffff 0%, #fafbfc 100%);"">
                                            <h1 class=""main-title"" style=""font-size: 32px; margin: 0 0 30px; color: #2c1810; text-align: center; font-weight: normal; position: relative;"">
                                                Welcome to Your Creative Journey!
                                                <div style=""width: 60px; height: 3px; background: linear-gradient(90deg, #d2691e, #ffd700); margin: 15px auto 0; border-radius: 2px;""></div>
                                            </h1>
                            
                                            <div class=""welcome-box"" style=""background: #f8f9ff; padding: 30px; border-radius: 12px; border-left: 4px solid #d2691e; margin-bottom: 30px;"">
                                                <p class=""welcome-text"" style=""font-size: 18px; line-height: 1.8; margin: 0 0 15px; color: #2c1810;"">
                                                    Hello <strong style=""color: #d2691e;"">{userName}</strong>,
                                                </p>
                                                <p class=""welcome-subtext"" style=""font-size: 16px; line-height: 1.8; margin: 0; color: #555;"">
                                                    Welcome to Legacy Loom, where digital innovation meets timeless craftsmanship. You're now part of a community that celebrates the art of storytelling through weaving, preserving traditions while creating new memories that will last for generations.
                                                </p>
                                            </div>

                                            <!-- Feature Highlights -->
                                            <div style=""margin: 40px 0;"">
                                                <div class=""feature-item"" style=""display: flex; align-items: center; margin-bottom: 20px; padding: 20px; background: #fff; border-radius: 10px; border: 1px solid #e9ecef; box-shadow: 0 2px 10px rgba(0,0,0,0.05);"">
                                                    <div class=""feature-icon"" style=""width: 50px; height: 50px; background: linear-gradient(135deg, #ff6b6b, #ff8e8e); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin-right: 20px; color: white; font-size: 20px;"">🧵</div>
                                                    <div>
                                                        <div class=""feature-title"" style=""font-weight: bold; color: #2c1810; margin-bottom: 5px;"">Craft Your Stories</div>
                                                        <div class=""feature-desc"" style=""color: #666; font-size: 14px;"">Transform your experiences into beautiful, woven narratives</div>
                                                    </div>
                                                </div>
                                
                                                <div class=""feature-item"" style=""display: flex; align-items: center; margin-bottom: 20px; padding: 20px; background: #fff; border-radius: 10px; border: 1px solid #e9ecef; box-shadow: 0 2px 10px rgba(0,0,0,0.05);"">
                                                    <div class=""feature-icon"" style=""width: 50px; height: 50px; background: linear-gradient(135deg, #4ecdc4, #7fdbda); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin-right: 20px; color: white; font-size: 20px;"">🏛️</div>
                                                    <div>
                                                        <div class=""feature-title"" style=""font-weight: bold; color: #2c1810; margin-bottom: 5px;"">Preserve Traditions</div>
                                                        <div class=""feature-desc"" style=""color: #666; font-size: 14px;"">Keep family heritage and cultural practices alive for future generations</div>
                                                    </div>
                                                </div>
                                
                                                <div class=""feature-item"" style=""display: flex; align-items: center; padding: 20px; background: #fff; border-radius: 10px; border: 1px solid #e9ecef; box-shadow: 0 2px 10px rgba(0,0,0,0.05);"">
                                                    <div class=""feature-icon"" style=""width: 50px; height: 50px; background: linear-gradient(135deg, #45b7d1, #74c0fc); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin-right: 20px; color: white; font-size: 20px;"">🤝</div>
                                                    <div>
                                                        <div class=""feature-title"" style=""font-weight: bold; color: #2c1810; margin-bottom: 5px;"">Connect & Share</div>
                                                        <div class=""feature-desc"" style=""color: #666; font-size: 14px;"">Join a community of creators and storytellers worldwide</div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- CTA Button -->
                                            <div style=""text-align: center; margin: 40px 0;"">
                                                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" style=""margin: 0 auto;"">
                                                    <tr>
                                                        <td style=""border-radius: 50px; background: linear-gradient(135deg, #d2691e 0%, #ffd700 100%); box-shadow: 0 8px 25px rgba(210, 105, 30, 0.3); transition: transform 0.3s ease;"">
                                                            <a class=""cta-button"" href=""https://example.com/get-started"" target=""_blank"" style=""display: inline-block; padding: 18px 40px; font-size: 18px; color: #ffffff; text-decoration: none; border-radius: 50px; font-weight: bold; letter-spacing: 0.5px; text-shadow: 1px 1px 2px rgba(0,0,0,0.2);"">
                                                                ✨ Begin Your Legacy ✨
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <!-- Support Section -->
                                            <div class=""support-section"" style=""text-align: center; padding: 30px; background: linear-gradient(135deg, #f8f9fa, #e9ecef); border-radius: 12px; margin-top: 30px;"">
                                                <div style=""font-size: 16px; color: #2c1810; margin-bottom: 15px;"">
                                                    <strong>Need assistance on your journey?</strong>
                                                </div>
                                                <p style=""font-size: 14px; line-height: 1.6; margin: 0; color: #666;"">
                                                    Our dedicated support team is here to help you every step of the way.<br>
                                                    Reach out to us at <a href=""mailto:rishavdaskaberipara@gmail.com"" style=""color: #d2691e; text-decoration: none; font-weight: bold;"">rishavdaskaberipara@gmail.com</a>
                                                </p>
                                            </div>
                                        </td>
                                    </tr>

                                    <!-- Footer -->
                                    <tr>
                                        <td class=""footer-padding"" style=""padding: 40px; background: linear-gradient(135deg, #2c1810 0%, #8b4513 100%); text-align: center; color: #ffffff;"">
                                            <div style=""margin-bottom: 20px;"">
                                                <div style=""font-size: 16px; font-weight: bold; margin-bottom: 10px;"">Legacy Loom</div>
                                                <div style=""font-size: 12px; opacity: 0.8; text-transform: uppercase; letter-spacing: 1px;"">Weaving the Future of Storytelling</div>
                                            </div>
                            
                                            <div style=""border-top: 1px solid rgba(255,255,255,0.2); padding-top: 20px;"">
                                                <p style=""font-size: 14px; margin: 0 0 15px; opacity: 0.9;"">
                                                    © 2025 Legacy Loom. All rights reserved.
                                                </p>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>"    
                #endregion
            };
            return templateContents[(int)templateName];
        }
        
    }
}
